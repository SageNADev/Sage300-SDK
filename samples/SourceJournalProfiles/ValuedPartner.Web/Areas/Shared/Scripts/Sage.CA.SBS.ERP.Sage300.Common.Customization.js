/* Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved. */

"use strict";
var Customization = Customization || { };
Customization = {
    html: "<div id='dlgCustomize'> <div id='grid'></div> <section class='footer-group'> <input class='btn btn-primary' id='btnCustomizeSave' type='button' value='" + globalResource.SaveTitle + "'><input class='btn btn-primary' id='btnCustomizeRestore' type='button' value='" + globalResource.Restore + "'></section> </div>",
    key: "aabe0981-a6a7-41af-adbe-fe480422325b",
    grid: null,
    defaultGridData: [],
    ignoreElements: ["chkGridPrefSelectAll", "btnGridPrefApply", "btnGridPrefCancel"],

    init: function() {
        Customization.showDialog();
        kendo.ui.plugin(SageNumericTextBoxPlugin);
    },

    set: function (model, root) {
        if (model) {
            var rootNode = root || window.document.body;
            $(rootNode).find("*[data-sg-default]").each(function() {
                var property = $(this);
                var defaultValue = property.attr("data-sg-default");
                //logic if the databind is applied
                var attribute = property.attr("data-bind")
                if (typeof attribute !== 'undefined' && attribute !== "") {
                    if (property.is(':text')) {
                        modelProperty = Customization.getModelProperty(model, $(this), "value")
                        if (modelProperty() == null || modelProperty() == "") {
                            modelProperty(defaultValue);
                        }
                    } else if (property[0].nodeName === "SELECT") {
                        property.find("option").each(function() {
                            if ($(this).text() === defaultValue) {
                                $(this).attr("selected", "selected");
                            }
                        });
                    } else if (property.is(':checkbox')) {
                        var modelProperty = Customization.getModelProperty(model, property, "sagechecked")
                        if (modelProperty() == null || modelProperty() == 0) {
                            modelProperty(defaultValue);
                        }
                    } else if (property.is(':radio')) {
                        if ($(this).attr("value") === defaultValue) {
                            var modelProperty = Customization.getModelProperty(model, property, "sagechecked")
                            if (modelProperty() == null || modelProperty() == 0) {
                                modelProperty(defaultValue);
                            }
                        }
                    }
                } else {
                    if (property.is(':text')) {
                        property.val(defaultValue);
                    } else if (property[0].nodeName === "SELECT") {
                        property.find("option").each(function() {
                            if ($(this).text() === defaultValue) {
                                $(this).attr("selected", "selected");
                            }
                        });
                    } else if (property.is(':checkbox')) {
                        property.prop('checked', defaultValue);
                    } else if (property.is(':radio')) {
                        if (property.attr("value") === defaultValue) {
                            property.prop('checked', true);
                        }
                    }
                }

            });
        }
    },

    getModelProperty: function (model, control, attrToCheck) {
        var databind = control.attr("data-bind");
        var properties = databind.split(',');
        var checkedProperty, modelProperty;
        $.each(properties, function (index, value) {
            checkedProperty = value.split(':');
            if (checkedProperty[0] === attrToCheck) {
                modelProperty = eval((model + "." + checkedProperty[1]));
                modelProperty;
            }
        });
        return modelProperty;

    },

    guid: function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    },

    getColTemplate: function (data) {
        var selected = data.Enabled ? "selected" : "";
        var checked = data.Enabled ? "checked" : "";
        var template = '<span class="icon checkBox ' + selected + '"><input class="Enabled" name="Enabled" type="checkbox" ' + checked + ' /> </span>';
        return (data.Type != "Label" && data.Show) ? template : "";
    },

    configGrid: function (gridData) {
        var grid = $("#grid").kendoGrid({
            dataSource: {
                data: gridData,
                schema: {
                    model: {
                        fields: {
                            Name: { type: "string", editable: false },
                            Type: { type: "string", editable: false },
                            Show: { type: "boolean", editable: true },
                            Text: { type: "string", editable: true  },
                            id:   { type: "string", editable: false }
                        },
                    }
                },
                pageSize: 10
            },
            height: 380,
            groupable: false,
            sortable: false,
            resizable: true,
            pageable: {
                input: true,
                numeric: false,
            },
            editable: false,
            columns: [
                {
                    title:  globalResource.Name,
                    field: "Name",
                    width: 200,
                },
                {
                    title: globalResource.Type,
                    field: "Type",
                    width: 100
                },
                {
                    title: globalResource.Show,
                    field: "Show",
                    width: 100,
                    template: '<span class="icon checkBox #=Show?"selected":""# "><input class="Show" name="Show" type="checkbox" #=Show?"checked":""# /> </span>',
                    //For testing purpose
                    //headerTemplate: '<input type="checkbox" checked id="showSelectAll" />Show'
                },
                {
                    title: globalResource.Text,
                    field: "Text",
                    width: 200,
                    //For testing purpose
                    //headerTemplate: '<input type="button" id="updateTextAll" value="Update Text" style="background-color:lightgreen"/>'
                },
                {
                    hidden: true,
                    field: "id",
                    width: 200,
                },
                {
                    hidden: true,
                    field: "changed",
                }
            ],
        }).data("kendoGrid");

        grid.bind("save", Customization.gridSave);

        grid.tbody.delegate(":checkbox", "change", function (e) {
            var checkBox = $(this);
            var colName = this.attributes["Name"].value;
            var model = grid.dataItem(checkBox.closest("tr"));
            model.set(colName, checkBox.is(":checked"));
            if(colName && colName ==="Show"){
                Customization.updateControl(model, true);
            } 
        });

        grid.tbody.delegate(":checkbox", "focus", function (e) {
            var checkBox = $(this);
            var colName = this.attributes["Name"].value;
            var model = grid.dataItem(checkBox.closest("tr"));
            if(colName === "Show") {
                model.set("Show", checkBox.is(":checked"));
            }
            if (colName === "Enabled" && model.Type === "Label") {
                e.preventDefault();
            } 
        });

        grid.tbody.on("click", "td", function (e) {
            grid.closeCell();
            var data = grid.dataItem($(e.target).closest("tr"));
            if(data) {
                if ((data.Type === "Label" || data.Type === "Button") && data.Text != "" && (!$.isNumeric(data.Text)) && e.target.cellIndex === 3) {
                    grid.editCell(e.target);
                }
            }
        });

        //For testing purpose
        $("#showSelectAll").change(function () {
            var state = $('#showSelectAll').is(':checked');
            $.each(grid.dataSource.data(), function () {
                if (this['Show'] != state){
                    this['Show'] = state;
                    Customization.updateControl(this, false);
                }
            });
            grid.refresh();

        });

        //For testing purpose
        $("#updateTextAll").click(function () {
            $.each(grid.dataSource.data(), function () {
                if (this['Text'] != "") {
                    this['Text'] = "ForTest";
                    Customization.updateControl(this, false);
                }
            });
            grid.refresh();
        });

        return grid;
    },

    getElement :function(control, byId){
        var elem = (byId) ? $('#' + control.id) : $("[data-sage300uicontrol*='type:" + control.Type + ',name:' + control.Name + "']");
        return elem;
    },

    updateControl: function (control, byId) {
        $("#btnCustomizeSave").prop('disabled', false);
        var elem = Customization.getElement(control, byId);
        if (elem.length > 0) {
            elem.css("display", control.Show ? "" : "none");
            if (control.Type === "Label" && (!$.isNumeric(control.Text))) {
                elem.text(control.Text);
            }
            if (control.Type === "Button") {
                elem.attr("value", control.Text);
                elem.text(control.Text);
            }
        }
    },

    //For future enable/disbale screen elements
    enableControl: function(control, byId){
        var elem = Customization.getElement(control, byId);
        if (elem.length > 0) {
            elem.prop("disabled", !control.Enabled);
        }
        // Handle special controls
        var expr = "[name ='" + control.Name + "']";
        if (control.Type == "Dropdown" && control.id) {
            var dropDown = $(expr).data("kendoDropDownList");
            if (dropDown) {
                dropDown.enable(control.Enabled);
            }
        }
        if (control.Type == "DatePicker" && control.id) {
            var datePicker = $(expr).data("kendoDatePicker");
            if (datePicker) {
                datePicker.enable(control.Enabled);
            }
        }
    },

    gridSave: function (e) {
        if (e.values.Text != e.model.Text) {
            e.model.Text = e.values.Text;
            Customization.updateControl(e.model, true);
        }
    },

    showDialog: function () {
        $('#btnCustomizeUI').bind('click', Customization.showWindow);
    },

    openWindow: function() {
        $('#dlgCustomize').kendoWindow({
            title: globalResource.Customize,
            height: '520px',
            width: '850px',
            draggable: true,
            show: 'blind',
            hide: 'blind',
            modal: true,
            resizable: false,

            open: function (e) {
                this.wrapper.css({ top: 100 });
            },

            close: function (event, ui) {
                $("#dlgCustomize").remove();
                this.destroy();
            }

        }).data("kendoWindow").center().open();
    },

    onEdit: function (e) {
        var data = e.model;
        var columnIndex = e.container.context.cellIndex;

        if (data.Type !== "Label" && data.Type !== "Button" && columnIndex === 3) {
            this.closeCell();
        }
        e.preventDefault();
    },

    getGridData: function () {
        var gridData = [];
        $("[data-sage300uicontrol]").each(function () {
            var elem = {};
            var bindingInfo = {};
            elem.Show = true;
            $($(this).attr("data-sage300uicontrol").split(",")).each(function (idx, binding) {
                var parts = binding.split(":");
                bindingInfo[parts[0].trim()] = parts[1].trim();
            });
            elem.changed = bindingInfo['changed'];
            elem.Name = bindingInfo['name'];
            if (elem.Name && Customization.ignoreElements.indexOf(elem.Name) > -1) {
                return true;
            }
            elem.Type = bindingInfo['type'];
            //set element text
            if (elem.Type == "Label") {
                elem.Text = $(this).text();
                if ($.isNumeric(elem.Text)) {
                    elem.Text = "";
                }
            } else if (elem.Type == "Button") {
                elem.Text = this.attributes['value'] ? this.attributes['value'].value : this.innerText;
            } else {
                elem.Text = "";
            }
            // set element id; if id exists, use it, otherwise, use the type+name as the id
            if ($(this).attr("id")) {
                elem.id = $(this).attr("id");
            } else {
                elem.id = Customization.guid();
                // add id to the element
                $(this).attr('id', elem.id);
            }
            if ($(this).css("display") === "none") {
                elem.Show = false;
            }
            //set enabled
            elem.Enabled = !$(this).prop("disabled");
            if ((elem.Type == "Dropdown" || elem.Type == "DatePicker") && elem.Name) {
                elem.Enabled = !$("[name = '" + elem.Name + "']").prop("disabled")
            }

            gridData.push(elem);
        });
        return gridData;
    },

    restore: function(){
        var data = { key: Customization.key, value: null };
        sg.utls.ajaxPostHtml(sg.utls.url.buildUrl("Core", "Common", "DeleteScreenPreference"), data, function () { window.location.reload() });
        $("#dlgCustomize").data("kendoWindow").close();
    },

    showWindow: function () {
        var gridData = [];

        $("body").append(Customization.html);
        $("#btnCustomizeSave").prop('disabled', true);

        $("#btnCustomizeSave").bind('click', function (e) {
            var dataSource = Customization.grid.dataSource.data();
            var ds = [];
            for (var i = 0, len = dataSource.length; i < len; i++) {
                var r = {};
                var elem = dataSource[i];
                if (!elem.dirty && elem.changed !="1") { continue; }
                r.Name = elem.Name;
                r.Hide = !elem.Show;
                r.Label = elem.Text;
                r.Type = elem.Type;
                ds.push(r);
            }
            var data = { key: Customization.key, value: ds };
            if (ds.length > 0){
                sg.utls.ajaxPostHtml(sg.utls.url.buildUrl("Core", "Common", "SaveScreenPreferences"), data);
            }
            $("#dlgCustomize").data("kendoWindow").close();
        });

        $("#btnCustomizeRestore").bind('click', function (e) {
            sg.utls.showKendoConfirmationDialog(function () { Customization.restore(); }, null, globalResource.RestoreMessage) ;
        });

        gridData = Customization.getGridData();
        Customization.grid = Customization.configGrid(gridData);
        Customization.openWindow();
    }
}

$(function () {
    Customization.init();
});
