/* Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved. */

"use strict";
var Customization = Customization || {};
Customization = {
    html: "<div id='dlgCustomize'> " +
        "<div class='form-group'>" +
        "<div class='plus-group'><label>" + globalResource.CustomizationID + "</label><input type='text' id='commonCustomizationID' value='' class='txt-upper small' formattextbox='alphaNumeric'><input type='button' id='btnCommonCustomizeCreate' class='icon btn-plus' tabindex='-1'><input type='button' id='btnCommonCustomizationIDFinder' class='icon btn-finder' tabindex='-1'></div>" +
        "<div class='input-group'><label>" + globalResource.CustomizationDescription + "</label><input type='text' id='commonCustomizationDescription' value='' class='medium-large'></div>" +
        "<div class='multiselect-group'><label>" + globalResource.UIProfiles + "</label><select class='large multi-select' id='commonCustomizeProfileID' multiple></select><input type='button' id='btnCommonCustomizeUIProfile' class='icon btn-more' tabindex='-1'/></div>" +
        "</div>" +
        "<div id='commonCustomizationGrid'></div>" +
        "<section class='footer-group-1'> <input class='btn btn-secondary' id='btnCommonCustomizeCancel' type='button' value='" + globalResource.CancelTitle + "'></input><input class='btn btn-primary' id='btnCommonCustomizeDelete' type='button' value='" + globalResource.DeleteTitle + "'></input><input class='btn btn-primary' id='btnCommonCustomizeSave' type='button' value='" + globalResource.SaveTitle + "'></input></section>" +
        "</div>" +
        "</div>",
    key: "aabe0981-a6a7-41af-adbe-fe480422325b",
    grid: null,
    defaultGridData: [],
    ignoreElements: ["chkGridPrefSelectAll", "btnGridPrefApply", "btnGridPrefCancel"],
    currentValue: [],
    UIMode: null,
    eTag: "",
    isModelDirty: false,
    finderData: null,
    lastCommonCustomizationID: null,

    init: function () {
        Customization.showDialog();
        kendo.ui.plugin(SageNumericTextBoxPlugin);
    },

    initFinders: function () {
        const finderProps = () => {
            const props = sg.utls.deepCopy(sg.viewFinderProperties.AS.Customizations);
            const screenName = $("#ScreenName").val().toUpperCase();
            props.finderTitle = jQuery.validator.format(globalResource.FinderTitle, globalResource.CustomizationID);
            props.filter = $.validator.format(props.filterTemplate, screenName);
            props.initKeyValues = [screenName, $('#commonCustomizationID').val().trim().toUpperCase()];
            return props;
        };
        sg.viewFinderHelper.setViewFinderEx("btnCommonCustomizationIDFinder", "commonCustomizationID", finderProps, Customization.customizationFinderSuccess);
    },

    customizationFinderSuccess: function (data) {
        if (data != null) {
            Customization.finderData = data;
            Customization.checkIsDirty(Customization.setCustomizationFinderData);
        }
    },

    setCustomizationFinderData: function () {
        $('#commonCustomizationID').val(Customization.finderData.CUSTID);
        $('#commonCustomizationDescription').val(Customization.finderData.DESCRIPT);
        $("#commonCustomizationID").trigger('change');
        sg.controls.Focus($("#commonCustomizationDescription"));
    },

    initCustomizationMultiSelect: function () {
        var fields = [{ field: "ProfileID", type: "string" }, { field: "ProfileDescription", type: "string" }];
        var model = sg.multiSelectionHelper.getModel("ProfileID", fields);
        sg.multiSelectionHelper.init("commonCustomizeProfileID", model, Customization.getMultiSelectTemplateData, sg.utls.url.buildUrl("AS", "WorkProfile", "GetWorkProfiles"), "PROFILEID", "CSUICSH", Customization.currentMultiSelectValue);
        var profilesMultiSelect = $("#commonCustomizeProfileID").data("kendoMultiSelect");
        if (profilesMultiSelect) {
            profilesMultiSelect.bind("change", Customization.profilesMultiSelectChange);
            profilesMultiSelect.dataSource.read();
        }
    },

    profilesMultiSelectChange: function (e) {
        Customization.isModelDirty = true;
    },

    getMultiSelectTemplateData: function (e) {
        var str;
        if (null == e.ProfileDescription)
            str = e.ProfileID;
        else
            str = e.ProfileID + ": " + e.ProfileDescription;

        return str.substring(0, 35);
    },

    set: function (model, root) {
        if (model) {
            var rootNode = root || window.document.body;
            $(rootNode).find("*[data-sg-default]").each(function () {
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
                        property.find("option").each(function () {
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
                        if ($(this).val() === defaultValue) {
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
                        property.find("option").each(function () {
                            if ($(this).text() === defaultValue) {
                                $(this).attr("selected", "selected");
                            }
                        });
                    } else if (property.is(':checkbox')) {
                        property.prop('checked', defaultValue);
                    } else if (property.is(':radio')) {
                        if (property.val() === defaultValue) {
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

    getShowColumnTemplate: function (data) {
        var selected = data? "selected" : "";
        var checked = data? "checked" : "";
        var val = $('#commonCustomizationID').val();
        if (!val)
            return '<label class="checkbox-container"><span class="' + selected + '"><input class="Show" disabled="disabled"  name="Show" type="checkbox"' + checked + ' /><span class="checkmark"></span></span></label>';
        else
            return '<label class="checkbox-container"><span class="' + selected + '"><input class="Show" name="Show" type="checkbox"' + checked + ' /><span class="checkmark"></span></span><label>';
    },

    configGrid: function (gridData) {
        var grid = $("#commonCustomizationGrid").kendoGrid({
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
                    template: '#= Customization.getShowColumnTemplate(Show) #',
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

        grid.tbody.on("change", ":checkbox", function (e) {
            var checkBox = $(this);
            var colName = this.attributes["Name"].value;
            var model = grid.dataItem(checkBox.closest("tr"));
            model.set(colName, checkBox.is(":checked"));
            if (colName && colName === "Show") {
                Customization.updateControl(model, true);
                Customization.isModelDirty = true;
            }
        });

        grid.tbody.on("focus", ":checkbox", function (e) {
            var checkBox = $(this);
            var colName = this.attributes["Name"].value;
            var model = grid.dataItem(checkBox.closest("tr"));
            if (colName === "Show") {
                model.set("Show", checkBox.is(":checked"));
            }
            if (colName === "Enabled" && model.Type === "Label") {
                e.preventDefault();
            }
        });

        grid.tbody.on("click", "td", function (e) {
            grid.closeCell();
            var val = $('#commonCustomizationID').val();
            if (!val) return;
            var data = grid.dataItem($(e.target).closest("tr"));
            if (data) {
                if ((data.Type === "Label" || data.Type === "Button") && data.Text != "" && (!$.isNumeric(data.Text)) && e.target.cellIndex === 3) {
                    grid.editCell(e.target);
                }
            }
        });

        //For testing purpose
        $("#showSelectAll").change(function () {
            var state = $('#showSelectAll').is(':checked');
            $.each(grid.dataSource.data(), function () {
                if (this['Show'] != state) {
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

    getElement: function (control, byId) {
        var elem = (byId) ? $('#' + control.id) : $("[data-sage300uicontrol*='type:" + control.Type + ',name:' + control.Name + "']");
        return elem;
    },

    updateControl: function (control, byId) {
        var elem = Customization.getElement(control, byId);
        if (elem.length > 0) {
            elem.css("display", control.Show ? "" : "none");
            var attribute = elem.attr("data-bind");
            if (!attribute && control.Type === "Label" && (!$.isNumeric(control.Text))){
                elem.text(control.Text);
            }
            if (control.Type === "Button") {
                elem.attr("value", control.Text);
                elem.text(control.Text);
            }
        }
    },

    //For future enable/disbale screen elements
    enableControl: function (control, byId) {
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
            Customization.isModelDirty = true;
        }
    },

    showDialog: function () {
        $('#btnCustomizeUI').bind('click', Customization.showWindow);
    },

    closeDialog: function () {
        $.each(Customization.defaultGridData, function () {
            Customization.updateControl(this, true);
        });

        $('#dlgCustomize').data("kendoWindow").destroy();
        $("#dlgCustomize").remove();
    },

    openWindow: function () {
        $('#dlgCustomize').kendoWindow({
            title: globalResource.Customize,
            width: '950px',
            draggable: true,
            show: 'blind',
            hide: 'blind',
            modal: true,
            resizable: false,
            visible: false,

            open: function (e) {
                Customization.isModelDirty = false;
                this.wrapper.css({ top: 100 });
                // For custom theme color
                sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
            },

            close: function (e) {
                e.preventDefault();
                $("#commonCustomizationDescription").trigger('blur');
                var val = $('#commonCustomizationID').val();
                if (Customization.isModelDirty && val) {
                    var customizationID = $('#commonCustomizationID').val().trim().toUpperCase();
                    var message = jQuery.validator.format(globalResource.SaveConfirm2, globalResource.Customization2 + " " + customizationID);
                    sg.utls.showKendoConfirmationDialog(
                        function () { // Yes
                            Customization.closeDialog();
                        },
                        function () { // No
                            return;
                        },
                        message);
                } else {
                    Customization.closeDialog();
                }
            }

                });

        var dialog = $('#dlgCustomize').data("kendoWindow");
        dialog.center().open();
        $("#commonCustomizationID").focus();

    },

    onEdit: function (e) {
        var data = e.model;
        var columnIndex = e.container[0].cellIndex;

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

    createNewCustomization: function () {
        sg.utls.ajaxPost(sg.utls.url.buildUrl("AS", "UICustomization", "Create"), null, Customization.createCustomizationHandler);
    },

    getCustomization: function () {
        var value = $("#commonCustomizationID").val().trim().toUpperCase();
        var data = {
            customizationID: value
        };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("AS", "UICustomization", "GetByID"), data, Customization.getCustomizationHandler);
    },

    checkIsDirty: function (funcionToCall) {
        var val = $('#commonCustomizationID').val();
        if (Customization.isModelDirty && val) {
            var customizationID = $('#commonCustomizationID').val().trim().toUpperCase();

            if ( Customization.lastCommonCustomizationID !== null &&
                 customizationID != Customization.lastCommonCustomizationID ) {
                 customizationID = Customization.lastCommonCustomizationID
            }

            var message = jQuery.validator.format(globalResource.SaveConfirm2, globalResource.Customization2 + " " + customizationID);
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes 
                    Customization.isModelDirty = false;
                    funcionToCall.call();
                },
                function () { // No
                    if ( Customization.lastCommonCustomizationID !== null ) {
                        customizationID = Customization.lastCommonCustomizationID;
                        $('#commonCustomizationID').val(customizationID)
                    }
                    return;
                },
                message);
        } else {
            funcionToCall.call();
        }
    },

    showWindow: function () {

        $("body").append(Customization.html);

        Customization.UIMode = sg.utls.OperationMode.NEW;

        Customization.initFinders();

        $("#commonCustomizationDescription").bind('change', function (e) {
            Customization.isModelDirty = true;
        });

        $("#commonCustomizationID").bind('change', function (e) {
            Customization.checkIsDirty(Customization.getCustomization);
            Customization.isModelDirty = true;
            if ($("#commonCustomizationID").val()) {
                $("#btnCommonCustomizeDelete").attr('disabled', false);
                $("#btnCommonCustomizeSave").attr('disabled', false);
            } else {
                $("#btnCommonCustomizeDelete").attr('disabled', true);
                $("#btnCommonCustomizeSave").attr('disabled', true);
            }
        });

        $("#btnCommonCustomizeSave").bind('click', function (e) {

            var profileData = $("#commonCustomizeProfileID").data("kendoMultiSelect").dataItems();
            var profiles = [];

            for (var i = 0; i < profileData.length; i++) {
                profiles.push(profileData[i].ProfileID);
            }

            var dataSource = Customization.grid.dataSource.data();
            var ds = [];
            for (var i = 0, len = dataSource.length; i < len; i++) {
                var r = {};
                var elem = dataSource[i];
                if (!elem.dirty && elem.changed != "1") { continue; }
                r.Name = elem.Name;
                r.Hide = !elem.Show;
                r.Label = elem.Text;
                r.Type = elem.Type;
                ds.push(r);
            }

            var customizationID = $('#commonCustomizationID').val().trim().toUpperCase();
            var customizationDescription = $('#commonCustomizationDescription').val();

            var data = {
                profileIDs: profiles,
                customizationID: customizationID,
                customizationDescription: customizationDescription,
                customizationDetail: ds,
                eTag: Customization.eTag
            };

            if (Customization.UIMode === sg.utls.OperationMode.NEW) {
                sg.utls.ajaxPost(sg.utls.url.buildUrl("AS", "UICustomization", "Add"), data, Customization.updateCustomizationHandler);
            }
            else {
                sg.utls.ajaxPost(sg.utls.url.buildUrl("AS", "UICustomization", "Save"), data, Customization.updateCustomizationHandler);
            }

        });

        $("#btnCommonCustomizeCreate").bind('click', function (e) {
            Customization.checkIsDirty(Customization.createNewCustomization);
        });

        $("#btnCommonCustomizeDelete").bind('click', function (e) {
            var customizationID = $('#commonCustomizationID').val().trim().toUpperCase();
            var message = jQuery.validator.format(globalResource.DeleteConfirm, globalResource.Customization2 + " " + customizationID);
            sg.utls.showKendoConfirmationDialog(function () {
                var data = {
                    customizationID: customizationID
                };
                sg.utls.ajaxPost(sg.utls.url.buildUrl("AS", "UICustomization", "Delete"), data, Customization.deleteCustomizationHandler);
            }, null, message);
        });

        $("#btnCommonCustomizeCancel").bind('click', function (e) {
            $("#dlgCustomize").data("kendoWindow").close();
        });

        $("#btnCommonCustomizeUIProfile").bind('click', function (e) {
            var url = sg.utls.url.buildUrl("AS", "WorkProfile", "Index") + "?guid=" + sg.utls.guid();

            sg.utls.iFrameHelper.openWindow("popupCustomizationUiProfile", "", url, 650, 1020);
        });

        Customization.initCustomizationMultiSelect();

        Customization.defaultGridData = Customization.getGridData();
        Customization.grid = Customization.configGrid(Customization.defaultGridData);
        $("#btnCommonCustomizeDelete").attr('disabled', true);
        $("#btnCommonCustomizeSave").attr('disabled', true);
        Customization.openWindow();
    },

    createCustomizationHandler: function (jsonResult) {
        Customization.displayResult(jsonResult, sg.utls.OperationMode.NEW);
        Customization.isModelDirty = false;
        sg.controls.Focus($("#commonCustomizationID"));
    },

    getCustomizationHandler: function (jsonResult) {
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            if (jsonResult.Data != null) {
                Customization.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            } else {
                Customization.UIMode = sg.utls.OperationMode.NEW;
            }
            
            Customization.lastCommonCustomizationID = $('#commonCustomizationID').val().trim().toUpperCase();
            sg.controls.Select($("#commonCustomizationDescription"));
        }
        Customization.grid.refresh();
        sg.utls.showMessage(jsonResult);
    },

    // update (post) call back handler 
    updateCustomizationHandler: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            Customization.UIMode = sg.utls.OperationMode.SAVE;
            Customization.isModelDirty = false;
        }
        sg.utls.showMessage(jsonResult);
    },

    // delete (post) call back handler 
    deleteCustomizationHandler: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            Customization.UIMode = sg.utls.OperationMode.NEW;
            Customization.isModelDirty = false;
            // clear the fields we just deleted
            $('#commonCustomizationID').val("");
            $('#commonCustomizationDescription').val("");
            var multiSelect = $("#commonCustomizeProfileID").data("kendoMultiSelect");
            multiSelect.value([]);
            var grid = $("#commonCustomizationGrid").data("kendoGrid");
            $.each(grid.dataSource.data(), function () {
                    this['Show'] = true;
            });
            grid.refresh();
        }
        sg.utls.showMessage(jsonResult);
    },

    displayResult: function (jsonResult, uiMode) {
        if (jsonResult != null) {
            Customization.isModelDirty = false;
            Customization.UIMode = uiMode;
            var customizationModel = jsonResult.Data;
            Customization.eTag = customizationModel.ETag;
            $('#commonCustomizationID').val(customizationModel.CustomizationID);
            $('#commonCustomizationDescription').val(customizationModel.Description);
            $("#commonCustomizeProfileID").data("kendoMultiSelect").value(customizationModel.UIProfiles);

            // Reset both the grid and screen controls
            Customization.grid.dataSource.data(Customization.defaultGridData);
            Customization.grid.dataSource.page(1);
            $.each(Customization.grid.dataSource.data(), function () {
                Customization.updateControl(this, true);
            });

            // update grid and screen controls based on customization detail
            Customization.applyCustomizationDetailToGrid(customizationModel.CustomizationDetail);
            Customization.grid.refresh();
        }
    },

    // Update the grid data source and screen controls if there are customized controls.
    // Normally that's the customization detail from backend.
    applyCustomizationDetailToGrid: function (customization) {
        if (!customization) { return; }

        var dataSource = Customization.grid.dataSource.data();

        customization.forEach(function (elem) {
            var key = elem.Name ? elem.Type + "_" + elem.Name : elem.Type;
            var elementPos = dataSource.map(function (x) { return x.Name ? x.Type + "_" + x.Name : x.Type; }).indexOf(key);
            if (elementPos === -1) { return; }

            var entry = dataSource[elementPos];
            
            if (entry) {
                entry.Show = !elem.Hide;
                entry.Text = elem.Label;
                entry.changed = "1";
                Customization.updateControl(entry, true);
            }
        });

    },

    populateMultiSelectFromPopup: function (profileId) {
        //Work in progress.

        //If ProfileId doesn't exist then multiselect will deselect it when it does the valueMapper call
        var multiSelect = $("#commonCustomizeProfileID").data("kendoMultiSelect");
        var selectedProfiles = multiSelect.value();

        // The change event for the multiselect widget is not triggered when the change is done via code. Thus,
        // when the widget is updated via the popup, the change event is manually triggered.
        if ($.inArray(profileId.toUpperCase(), selectedProfiles) == -1) {
            multiSelect.trigger("change");
        }

        selectedProfiles.push(profileId.toUpperCase());
        multiSelect.value([]);
        multiSelect.dataSource.read();
        multiSelect.value($.unique(selectedProfiles));

    },

    onIFrameHelperMessage: function (event) {
        if (event.data.Type == "SageKendoiFrame") {
            if (event.data.Id == "WorkProfile") {
                if (event.data.Data.ProfileId) {
                    Customization.populateMultiSelectFromPopup(event.data.Data.ProfileId);
                }
            }
        }
    },
};

$(function () {
    Customization.init();
    sg.utls.iFrameHelper.registerToReceiveMessage(Customization.onIFrameHelperMessage);
});
