/* Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved. */

"use strict";

/* Label Menu Popup Customization */
var LabelMenuHelper = LabelMenuHelper || {};
LabelMenuHelper = {

    screenModelName: null,

    initialize: function (data, btnHamburger, viewModelName) {

        $(document).off('.labelMenu');
        ko.cleanNode($("#divLabelMenu")[0]);
        $("#" + btnHamburger).attr('tabindex', '-1');

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

        // Mouse leave event
        $(document).contents().find('[class^="label-menu"]').on('mouseleave.labelMenu', function (e) {
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

                var isPortal = window.top.$('iframe.screenIframe:visible').length > 0;
                var parentForm = (isPortal) ? window.top.$('iframe.screenIframe:visible').contents().find('form:first') : window.top;
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
}
