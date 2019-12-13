// Copyright (c) 2018 Sage Software, Inc.  All rights reserved.
"use strict"

var globalSearchUI = globalSearchUI || {};
globalSearchUI = {
    globalSearchModel: {
        data: ko.observableArray(),
    },
    dataSourceConfig: {
        transport: {
            read: function (options) {
                globalSearchUI.search(options);
            }
        },
        pageSize: globalSearchPageSize,
        serverPaging: true,
        schema: {
            data: function (response) {
                return response.Data;
            },
            total: function (response) {
                return response.Total;
            }
        }
    },
    init: function () {
        //TODO: Enable autoComplete
        //globalSearchUI.initAutoComplete();
        globalSearchUI.initButton();
        globalSearchUI.initListView();
        globalSearchUI.initTreeView();
    },

    initTreeView: function() {
        $("#searchableEntityTreeView").kendoTreeView({
            checkboxes: {
                checkChildren: true
            },
            
            dataSource: entitySchemaList
        });

        // default all checkboxes are checked and expand the first level
        var treeview = $("#searchableEntityTreeView").data("kendoTreeView");
        var firstItem = treeview.dataItem(".k-item:first");
        if (firstItem) {
            firstItem.set("checked", true);
            if (firstItem.items && firstItem.items.length > 0) {
                treeview.expandTo(firstItem.items[firstItem.items.length - 1]);
            }
        }
    },

    initListView: function () {
        var ds = new kendo.data.DataSource(globalSearchUI.dataSourceConfig);

        $("#bodySearchResultListView").kendoListView({
            dataSource: ds,
            template: kendo.template($("#template").html()),
            autoBind: false,
            dataBound: globalSearchUI.initSearchResult
        });

        $("#searchResultPager").kendoPager({
            dataSource: ds,
            numeric: true,
            autoBind: false
        });
    },

    initSearchResult: function () {
        if (this.dataSource.data().length === 0) {
            $("#resultMessage").text($.validator.format(globalSearchResources.NoResult, $("#globalSearchBox").val()));
            $("#resultMessage").show();
        } else {
            $("#resultMessage").hide();
        
            $(".item-ID").click(function () {
                var webDetailInfoArray = this.dataset.webdetailinfo.split(";"); // <screenid>;<parameter name>;<parameter value>; ... <parameter name>;<parameter value>;
                var breadCrumbManager = window.parent.taskDockMenuBreadCrumbManager;

                breadCrumbManager.setGlobalSearchDrillDownParameter("");

                // To make sure the remaining values except the screen id exist in pairs.
                if ((webDetailInfoArray.length - 1) % 2 === 0) { 

                    var params = "";
                    for (var x = 1; x < webDetailInfoArray.length; x++) {
                        if (params !== "") {
                            params += "&";
                        }
                        params += webDetailInfoArray[x] + "=" + webDetailInfoArray[++x];
                    }

                    breadCrumbManager.setGlobalSearchDrillDownParameter(params);
                }
                $("a[data-menuid=" + webDetailInfoArray[0] + "]", window.parent.document.body)[0].click();
            });
        }
    },

    initButton: function () {
        $("#btnSearchFind").click(function () {

            var values = [];
            globalSearchUI.getCheckedEntity($("#searchableEntityTreeView").data("kendoTreeView").dataSource.view(), values);
            if (values.length == 0) {
                sg.utls.showKendoMessageDialog(function () { }, globalSearchResource.noSelection);
            } else {
                if ($("#searchResultPager").data().kendoPager.page() === 0) {
                    $("#bodySearchResultListView").data().kendoListView.dataSource.read();
                } else {
                    // set pager to 1 will set the pager back to 1 (as new search is about to happen) 
                    // and trigger the datasource to read again which as the result, will to search again
                    $("#searchResultPager").data().kendoPager.page(1);
                }
            }
        });

        $("#globalSearchBox").keyup(function (event) {
            if (event.keyCode === 13) {
                $("#btnSearchFind").click();
            }
        });

        $("#btnResetCompany").click(function () {
            sg.utls.showKendoConfirmationDialog(
                function() { // Yes
                    globalSearchRepository.ResetCompany({},
                        function(result) {
                            // result could be true or false, what to do about it?
                        }
                    );
                },
                function() { }, // No
                globalSearchResource.resetConfirm // the message
            );
        });
    },

    initAutoComplete: function () {
        var content = '# for (var i = 1; i < data.Path.length; i++) { ## if (i > 1) { # - # } ##: data.Path[i] ## } #' +
            '# for (var j = 0; j < data.Fields.length; j++) { # - #: data["FieldName-" + data.Fields[j]] #: #: data[data.Fields[j]] ## } #';
        var autocomplete = $("#globalSearchBox").kendoAutoComplete({
            minLength: 2,
            template:
                '<span class="k-state-default" style="background-image: url(\'../../../Assets/images/search/#:data.Type#.png\')"></span>' +
                '<span class="k-state-default" title="' + content + '">' +
                '<h3>#: data["FieldName-"+data.Header] #: #: data[data.Header] #</h3>' +
                '<p>' + content + '</p></span>',
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: {
                        url: function() {
                            return sg.utls.url.buildUrl("Core", "GlobalSearch", "Search");
                        },
                        data: function() {
                            return { query: $("#globalSearchBox").val(), }
                        },
                    }
                }
            },
            //TODO: change select item behaviour
            dataTextField: "customer_number",
            height: 408,
            clearButton: false
        }).data("kendoAutoComplete");
    },

    search: function (options) {
        // use .main-body as indicator on the status of the page
        if (!$('.main-body').hasClass('result-active')) {
            $('.main-body').addClass('result-active');
            $(".body-search-result").show();
        }

        var values = [];
        globalSearchUI.getCheckedEntity($("#searchableEntityTreeView").data("kendoTreeView").dataSource.view(), values);
        var data = {
            "query": $("#globalSearchBox").val(),
            "entities": values,
            "pageSize": options.data.pageSize,
            "pageNumber": options.data.page
        };
        
        globalSearchRepository.Search(data,
            function (result) {
                if (result) {
                    if (result.UserMessage) {
                        sg.utls.showMessage(result);
                    } else {
                        options.success(result);
                    }
                }
            }
        );
    },
    getCheckedEntity: function(nodes, checkedNodes) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].checked && !nodes[i].header) {
                checkedNodes.push(nodes[i].id);
            }

            if (nodes[i].hasChildren) {
                globalSearchUI.getCheckedEntity(nodes[i].children.view(), checkedNodes);
            }
        }
    }
}

$(document).ready(function () {
    globalSearchUI.init();

    $(window).on('beforeunload', function () {
        $(parent.document.getElementById("globalSearch")).removeClass("active");
    });
});
