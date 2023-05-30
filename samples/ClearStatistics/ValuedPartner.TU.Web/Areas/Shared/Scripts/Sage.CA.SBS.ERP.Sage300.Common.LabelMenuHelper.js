/* Copyright (c) 1994-2015 Sage Software, Inc.  All rights reserved. */

"use strict";

/* Label Menu Popup Customization */
var LabelMenuHelper = LabelMenuHelper || {};
LabelMenuHelper = {

    screenModelName: null,

    initialize: function (data, btnHamburger, viewModelName) {
        $(document).off('.labelMenu');
        ko.cleanNode($("#divLabelMenu")[0]);
        $("#" + btnHamburger).attr('tabindex', '0');

        LabelMenuHelper.screenModelName = viewModelName;

        //Get UI object view model from viewModelName string, the viewModelName like this "{UIObject}.{viewModelName}" 
        function getViewModel(viewModelName) {
            return viewModelName ? viewModelName.split('.').reduce(function (obj, i) { return obj[i]; }, window) : null;
        };

        var viewModel = getViewModel(viewModelName);

        // Mouse enter event
        $("#" + btnHamburger).on('mouseenter.labelMenu', viewModel, function (e) {
            // Load Menu
            var ulMenu = $('#lstLabelMenu');
            ulMenu.empty();
            for (var i = 0; i < data.length; i++) {
                ulMenu.append(window.hamburgerElements.menuItem);
                var val = data[i];
                var btnId = val.Id;
                var btn = ulMenu.find("#btnNewHamburgerMenu").attr({
                    "id": val.Id,
                    "name": val.Id,
                    "class": "action-btn",
                    "value": val.Value,
                    "data-bind": val.koAttributes
                }).on("click", val.callback);
            }

            LabelMenuHelper.ShowMenu($('#divLabelMenu'), $(this));
            $("#divLabelMenu").appendTo($("#" + btnHamburger));

            if (jQuery.isEmptyObject(viewModel)) {
                viewModel = getViewModel(LabelMenuHelper.screenModelName);
            }

            if (viewModel) {
                ko.applyBindings(viewModel, $("#divLabelMenu")[0]);
            }
        });

        // Mouse enter event
        $("#" + btnHamburger).on('keydown', viewModel, function (e) {
            if (e.target.id !== btnHamburger) {
                return;
            }

            if (e.which === sg.constants.KeyCodeEnum.Enter) {
                e.preventDefault();

                // Load Menu
                var ulMenu = $('#lstLabelMenu');
                ulMenu.empty();
                for (var i = 0; i < data.length; i++) {
                    ulMenu.append(window.hamburgerElements.menuItem);
                    var val = data[i];
                    var btnId = val.Id;
                    var btn = ulMenu.find("#btnNewHamburgerMenu").attr({
                        "id": val.Id,
                        "name": val.Id,
                        "class": "action-btn",
                        "value": val.Value,
                        "data-bind": val.koAttributes
                    }).on("click", val.callback);

                    $('#' + btnId).on('keydown', function (ex) {
                        if (ex.which === sg.constants.KeyCodeEnum.Enter) {
                            ex.preventDefault();
                            LabelMenuHelper.HideMenu(ex);
                            $('#' + btnId).trigger('click');
                        }
                        else if (ex.which === sg.constants.KeyCodeEnum.Tab) {
                            if (ex.currentTarget.closest("ul").lastElementChild.firstChild.id === ex.currentTarget.id) { //If tabbing out of last element, simply close the menu
                                LabelMenuHelper.HideMenu(ex);
                            }
                        }
                    });
                }

                LabelMenuHelper.ShowMenu($('#divLabelMenu'), $(this));
                $("#divLabelMenu").appendTo($("#" + btnHamburger));

                if (jQuery.isEmptyObject(viewModel)) {
                    viewModel = getViewModel(LabelMenuHelper.screenModelName);
                }

                if (viewModel) {
                    ko.applyBindings(viewModel, $("#divLabelMenu")[0]);
                }
            }
        });

        // Mouse leave event
        $(document).contents().find('[class^="label-menu"]').on('mouseleave.labelMenu', function (e) {
            LabelMenuHelper.HideMenu(e);
        });

        // Menu Item click event
        $(document).on('click.labelMenu', function (e) {
            var container = $('#divLabelMenu');
            // if the target of the click isn't the container... nor a descendant of the container
            if (!container.is(e.target) && container.has(e.target).length === 0) {
                // Detach and Append the container (div) to the current parent, 
                // because this container not gets scrolled along with the page when loaded inside the popup
                if (!sg.utls.isSameOrigin()) {
                    return;
                }
                var iFrame = window.top.$('iframe.screenIframe:visible');
                var isPortal = iFrame && iFrame.length > 0;
                var parentForm = isPortal ? iFrame.contents().find('form:first') : window.top;
                var kendoWindowContainer = container.closest('.k-window-content.k-content');
                if (parentForm !== null && parentForm.length > 0 && kendoWindowContainer !== null && kendoWindowContainer.length > 0) {
                    container.detach();
                    parentForm.append(container);
                }
            }
        });
    },

    /**
     * Display the menu items.
     * @method ShowMenu      
     * @param {} container - Parent div element.
     * @param {} e - anchor button.
     */
    ShowMenu: function (container, e) {
        if (e.offset() !== null && e.attr('id') != "divLabelMenu") {
            // show the menu
            container.addClass("show");
            container.removeClass("hide");
        }
    },

    HideMenu: function (e) {
        var container = $('#divLabelMenu');
        var list = $('#divLabelMenu ul:first');
        if ($(this).is(e.relatedTarget) || container.is(e.relatedTarget) || list.is(e.relatedTarget)) {
            LabelMenuHelper.ShowMenu(container, $(this));
            $("#divLabelMenu").appendTo($("#" + btnHamburger));
        }
        else {
            // hide the menu
            container.addClass("hide");
            container.removeClass("show");
        }
    }
}
