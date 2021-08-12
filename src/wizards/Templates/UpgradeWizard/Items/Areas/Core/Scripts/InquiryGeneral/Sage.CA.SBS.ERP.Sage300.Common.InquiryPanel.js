/* Copyright (c) 2016-2017 Sage Software, Inc.  All rights reserved. */

/* global kendo */
/* exported InquiryGeneralUI */
/* global InquiryGeneralViewModel */
/* global savedQueryResources*/
/* gloabl inquiryGeneralResources*/

"use strict";
var inquiryPanelUI = inquiryPanelUI || {};
inquiryPanelUI = {
    inquiryKoBindingModel: {},
    queryTemplate: null,
    templateId: null,
    init: function () {
        $(".tab-group").kendoTabStrip({
            animation: {
                open: {
                    effects: "fadeIn"
                }
            }
        });

        window.addEventListener('message', function (event) {
            if (event.data.event_id === 'OpenQuery') {
                openQueryClicked();
            } else if (event.data.event_id === 'SaveQuery') {
                inquiryPanelUI.templateId = event.data.templateId;
                saveQueryClicked();
            } else if (event.data.event_id === 'SaveTemplate' || event.data.event_id === 'DeleteTemplate') {
                var ddDataSource = $("#DataSourceList").data("kendoDropDownList");
                ddDataSource.trigger("change");
            } else if (event.data.event_id === 'QueryDataSourceChanged') {
                var dataSourceId = event.data.dataSourceId;
                var ddDataSource = $("#DataSourceList").data("kendoDropDownList");
                ddDataSource.value(dataSourceId);
                ddDataSource.trigger("change");
            }
               
        });

        function receiveMessage(event) {
            saveQueryClicked();
        }

        function receiveMessageOpen(event) {
            openQueryClicked();
        }

        function saveQueryClicked() {
            $('#saveQueryPanel').addClass('slide-in');
        }

        function openQueryClicked() {
            $('#openQueryPanel').addClass('slide-in');
        }

        inquiryPanelUI.initDropDownList();
        inquiryPanelUI.initButton();
        inquiryPanelUI.initGrid();
        inquiryPanelUI.initCheckBox();
        //$("#dateModified").prop("readonly", true);
        //$("#dateModified").val(Date().toLocaleDateString());

    
    },

    initCheckBox : function(){
        $("#divChkShared > span").addClass("selected");
        $("#divChkPersonal > span").addClass("selected");
        $("#chkPublic").prop("checked", true);
        $("#chkPrivate").prop("checked", true);
    },

    initDropDownList: function () {
        sg.utls.kndoUI.dropDownList("SaveQueryPanel_QueryTypeList");
        $("#SaveQueryPanel_QueryTypeList").kendoDropDownList();
        sg.utls.kndoUI.dropDownList("DataSourceList");
        $("#DataSourceList").kendoDropDownList({
            change: function (e) {
                var id = this.dataItem(e.item).DataSourceId;
                var url = sg.utls.url.buildUrl("Core", "InquiryGeneral", "GetTemplates");
                sg.utls.ajaxPost(url, { dataSourceId : id }, inquiryPanelUI.populateTemplate);
            },
        });
    },

    populateDropdownList: function (data) {
        var ddDataSource = $("#DataSourceList").data("kendoDropDownList");
        ddDataSource.setOptions({ dataTextField: "DataSourceText", dataValueField: "DataSourceId" });
        ddDataSource.setDataSource(data);
        if (data.length > 0) {
            ddDataSource.select(0);
            ddDataSource.trigger("change");
        }
    },

    populateTemplate: function (data) {
        if (data && data.length > 0) {
            var template = {};
            template.DataSourceId = data[0].DataSourceId;
        }
        var commonTemplate = $("#commonTemplateList");
        var customizedTemplate = $("#customizedTemplateList");

        var commonList = data.filter(function (item) { return item.Type.trim() == "Template"; } );
        var publicList = data.filter(function (item) { return item.Type.trim() == "Public"; });
        var privateList = data.filter(function (item) { return item.Type.trim() == "Private" ;});
        var customList = publicList.concat(privateList);

        inquiryPanelUI.AddlistToTab(commonTemplate, commonList, 0);
        inquiryPanelUI.AddlistToTab(customizedTemplate, customList, 1);

        //set save open panel grid data source
        var dsSaveData = publicList.concat(privateList)
        dsSaveData = dsSaveData.map(function (i) {
            if (i.Type.trim() == "Public") {
                i.Type = savedQueryResources.Public;
            } else if (i.Type.trim() == "Private") {
                i.Type = savedQueryResources.Private;
            }
            return i;
        });

        var dsOpenData = commonList.concat(dsSaveData);
        dsOpenData = dsOpenData.map(function (i) {
            if (i.Type.trim() == "Template") {
                i.Type = savedQueryResources.Template;
            } 
            return i;
        });

        var dsSave = new kendo.data.DataSource({ data: dsSaveData });
        var gridSave = $("#saveQueryPanelGrid").data("kendoGrid");
        gridSave.setDataSource(dsSave);

        var dsOpen = new kendo.data.DataSource({ data: dsOpenData });
        var gridOpen = $("#openQueryPanelGrid").data("kendoGrid");
        gridOpen.setDataSource(dsOpen);

        if (customList.length === 0) {
            $("#message-info").show();
        } else {
            $("#message-info").hide();
        }

        inquiryPanelUI.initCheckBox();

        var ddDataSource = $("#DataSourceList").data("kendoDropDownList");
        var dataSourceName = ddDataSource.text();
        $("#SaveQueryPanel_DataSource").val(dataSourceName);
        $("#openQueryPanel_DataSource").val(dataSourceName);
    },

    AddlistToTab: function(element, list, showTag)
    {
        element.empty();
        var template = "<li class='item-list' id='{0}' templateId={1} name='{2}'><label title='{3}'>{3}{span}</label></li>";
        for (var i = 0, length = list.length; i < length; i++) {
            var span = list[i].Type.trim() == "Public" ? "<span class='tag tag-pill tag-default'>Public</span>" : "<span class='tag tag-pill tag-info'>Private</span>";
            var tElement = template.replace("{span}", (showTag) ? span : "");
            var subElement = kendo.format(tElement, list[i].DataSourceId, list[i].TemplateId, list[i].Name, kendo.htmlEncode(list[i].Name));
            element.append(subElement);
        }

        var li = $("#" + element[0].id + " li")
        li.off('click');
        li.click(function () {
            var parameterData = $(this).data();
            var dsName = $(this).attr("name"); //$(this).text();
            parameterData["title"] = dsName;
            parameterData["url"] = sg.utls.url.buildUrl("Core", "InquiryGeneral", "Index");
            parameterData["name"] = $(this)[0].attributes['name'].value;
            parameterData["id"] = $(this)[0].attributes['id'].value;
            parameterData["templateId"] = $(this)[0].attributes['templateId'].value;

            var parameterDataString = JSON.stringify(parameterData);
            window.top.postMessage("isInquiryGeneral" + " " + encodeURI(parameterDataString), "*");

            var ddDataSource = $("#DataSourceList").data("kendoDropDownList");
            var dataSourceName = ddDataSource.text();
            $("#SaveQueryPanel_DataSource").val(dataSourceName);
            $("#openQueryPanel_DataSource").val(dataSourceName);
            $("#btnInquiryClose").trigger("click");
        });
    },

    initButton: function () {

        $("#chkPublic").click(function () {
            $("#customizedTemplateList li").has("span.tag-default").toggle();
        });

        $("#chkPrivate").click(function () {
            $("#customizedTemplateList li").has("span.tag-info").toggle();
        });

        $("#btnViewInquiries").click(function () {
            $('#viewInquiries').addClass('slide-in');
            var ddDataSource = $("#DataSourceList").data("kendoDropDownList");
            var data = ddDataSource.dataSource.data();
            if (data.length == 0) {
                var url = sg.utls.url.buildUrl("Core", "InquiryGeneral", "GetDataSources");
                sg.utls.ajaxPost(url, null, inquiryPanelUI.populateDropdownList);
            }
        });

        $('#btnInquiryClose').click(function () {
            $('#viewInquiries').removeClass("slide-in");
        });

        $('#btnSaveQueryClose, #btnSaveQueryCancel').click(function () {
            hideSavePanel();
        });

        function hideSavePanel() {
            $('#saveQueryPanel').removeClass("slide-in");
            $('#SaveQueryPanel_QueryName').val('');
            $('#SaveQueryPanel_QueryDescription').val('');
        }

        function hideOpenPanel() {
            $('#openQueryPanel').removeClass("slide-in");
            $('#OpenQueryPanel_QueryDescription').val('');
        }

        $('#btnOpenQueryClose, #btnOpenQueryCancel').click(function () {
            hideOpenPanel();
        });

        $("#SaveQueryPanel_QueryName").change(function () {
            $("#btnSaveQuerySave").prop("disabled", $(this).val().length == 0);
        });

        $("#btnSaveQuerySave").prop("disabled", true);

        $('#btnSaveQuerySave').click(function () {
            var grid = $("#saveQueryPanelGrid").data("kendoGrid");
            var data = grid.dataSource.data();
           
            var queryName = $("#SaveQueryPanel_QueryName").val();
            var queryNameList = data.map(function (item) { return item.Name.toLowerCase(); });

            if (queryName) {
                if (queryNameList.indexOf(queryName.toLowerCase()) > -1) {
                    var message = savedQueryResources.DuplicateMessage.replace('{0}', queryName);
                    sg.utls.showKendoConfirmationDialog(function () {
                        postSaveMessage(queryName);
                    },
                    null, message, "");
                } else {
                    postSaveMessage(queryName);
                }
            }
        });

        function postSaveMessage(queryName) {
            var grid = $("#saveQueryPanelGrid").data("kendoGrid");
            var selectedRow = sg.utls.kndoUI.getSelectedRowData(grid);
            var value = {};
            value.Type = $("#SaveQueryPanel_QueryTypeList").val();
            value.Name = queryName;
            value.Description = $("#SaveQueryPanel_QueryDescription").val();
            if (selectedRow) {
                value.TemplateId = selectedRow.TemplateId;
            }
            value.winTemplateId = inquiryPanelUI.templateId;
            window.postMessage({ event_id: 'SaveQueryPanel', template: value }, '*');
            hideSavePanel();
        };

        $('#btnOpenQuery').click(function () {
            var grid = $("#openQueryPanelGrid").data("kendoGrid");
            var selectedRowData = sg.utls.kndoUI.getSelectedRowData(grid);
            if (selectedRowData == null ) {
                return;
            }
            var parameterData = {};
            parameterData["title"] = selectedRowData.Description;
            parameterData["url"] = sg.utls.url.buildUrl("Core", "InquiryGeneral", "Index");
            parameterData["name"] = selectedRowData.Name;
            parameterData["id"] = selectedRowData.DataSourceId;
            parameterData["templateId"] = selectedRowData.TemplateId;

            var parameterDataString = JSON.stringify(parameterData);
            window.top.postMessage("isInquiryGeneral" + " " + encodeURI(parameterDataString), "*");
            hideOpenPanel();
            grid.clearSelection();

        });

        $("#inquiryTool1").click(function () {
            $('#viewInquiries').removeClass("slide-in");
            $('#iframeMenu1').show();
            $('#breadcrumbWrapper').show();
        });
    },

    initGrid: function () {
        function initKendoGrid(id) {
            $(id).kendoGrid({
                dataSource: {
                    data: [],
                },
                scrollable: true,
                sortable: true,
                height: 300,
                selectable: true,
                resizable: true,
                columns: [
                    {
                        field: "Name",
                        title: savedQueryResources.QueryName,
                        width: 170
                    },
                    {
                        field: "Description",
                        title: savedQueryResources.Description,
                        hidden: true
                    },
                    {
                        field: "Type",
                        title: savedQueryResources.Type,
                        width: 70
                    },
                    {
                        field: "DateModified",
                        title: savedQueryResources.DateModified,
                        template: '#= sg.utls.kndoUI.getFormattedDate(DateModified) #',
                    }
                ],
                editable: false,
                change: function () {
                    var grid = $(id).data("kendoGrid");
                    var selectedRowData = sg.utls.kndoUI.getSelectedRowData(grid);
                    if (selectedRowData) {
                        if (id == "#saveQueryPanelGrid") {
                            $("#SaveQueryPanel_QueryName").val(selectedRowData.Name);
                            $("#SaveQueryPanel_QueryDescription").val(selectedRowData.Description);
                            //$("#SaveQueryPanel_QueryTypeList").val(selectedRowData.Type.trim());
                            //$("#dateModified").val(sg.utls.kndoUI.getFormattedDate(selectedRowData.DateModified));
                            $("#btnSaveQuerySave").prop("disabled", false);
                        }
                        if (id == "#openQueryPanelGrid") {
                            $("#OpenQueryPanel_QueryDescription").val(selectedRowData.Description);
                        }
                    }
                }
            });
        }

        initKendoGrid("#saveQueryPanelGrid");
        initKendoGrid("#openQueryPanelGrid");
        $("#openQueryPanelGrid").on("dblclick", "tbody>tr", function () {
            $('#btnOpenQuery').click();
        });
    },

};

$(function () {
    inquiryPanelUI.init();
});
