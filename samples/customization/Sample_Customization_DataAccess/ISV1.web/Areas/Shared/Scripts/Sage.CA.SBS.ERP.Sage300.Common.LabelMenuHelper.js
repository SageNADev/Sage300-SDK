// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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

        // Mouse enter event
        $("#" + btnHamburger).on('mouseenter.labelMenu', function (e) {

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

            var viewModel = eval(LabelMenuHelper.screenModelName);
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
                var parentForm = window.top.$('iframe.screenIframe:visible').contents().find('form:first');
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
