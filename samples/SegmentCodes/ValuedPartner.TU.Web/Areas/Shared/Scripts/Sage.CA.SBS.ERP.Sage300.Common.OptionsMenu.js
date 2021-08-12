/* Copyright (c) 2020 Sage Software, Inc.  All rights reserved. */

"use strict";
/**
@module sg
*/
var sg = sg || {};

/**
@class sg.utls
*/
sg.utls = sg.utls || {};

/**
@class sg.utls.OptionsMenu
*/
sg.utls.OptionsMenu = sg.utls.OptionsMenu || {};

$.extend(sg.utls.OptionsMenu, {

    /**
     * This ID should match the one in _OptionsMenu.cshtml
     */
    menuId: 'OptionsMenu',

    /**
     * @name getOptionsMenu
     * @description Get a reference to the pages Options Menu
     * @namespace sg.utls.OptionsMenu
     * @public
     * 
     * @returns {object} Returns a reference to the pages Options Menu
     */
    getOptionsMenu: function () {
        return $('#' + sg.utls.OptionsMenu.menuId);
    },

    /**
     * @name _fixCSS
     * @description Ensure that the CSS classes k-first and k-last are correctly placed after insertion/deletion of items
     *              Developer Note: This is meant to be called by other methods, not called directly.
     * @namespace sg.utls.OptionsMenu
     * @private
     */
    _fixCSS: function () {
        var menu = sg.utls.OptionsMenu.getOptionsMenu();
        if (menu) {
            var allMenuItems = $(menu).children();
            var menuCount = allMenuItems.length;

            $.each(allMenuItems, function (index, value) {

                // Doesn't matter if this is a regular top-level menu item
                // or a SubMenu
                sg.utls.OptionsMenu._fixItemClass(menuCount, index, value);

                // Is this item a SubMenu?
                // Process SubMenu items
                if (value.classList.contains('menu-with-submenu')) {

                    // Find the ul container 
                    var liItems = $(value).find('li');
                    var subMenuItemCount = liItems.length;
                    $.each(liItems, function (i, v) {
                        sg.utls.OptionsMenu._fixItemClass(subMenuItemCount, i, v);
                    });
                } 
            });
        }
    },

    /**
     * @name _fixItemClass
     * @description Ensure the list of list items have their classes correctly specified
     *              First item has 'k-first' and last item has 'k-last'
     * @namespace sg.utls.OptionsMenu
     * @private
     * 
     * @param {number} itemCount The number of items in the array
     * @param {number} index The index of the array of items
     * @param {object} item The item reference
     */
    _fixItemClass: function (itemCount, index, item) {

        const firstClassName = 'k-first';
        const lastClassName = 'k-last';

        // First Item?
        if (index === 0) {
            if (!item.classList.contains(firstClassName)) {
                $(item).addClass(firstClassName);
            }
            if (item.classList.contains(lastClassName)) {
                $(item).removeClass(lastClassName);
            }
        }

        // Not first or last item
        if (index >= 1 && index < itemCount - 1) {
            if (item.classList.contains(firstClassName)) {
                $(item).removeClass(firstClassName);
            }
            if (item.classList.contains(lastClassName)) {
                $(item).removeClass(lastClassName);
            }
        }

        // Last Item?
        if (index === itemCount - 1) {
            if (item.classList.contains(firstClassName)) {
                $(item).removeClass(firstClassName);
            }
            if (!item.classList.contains(lastClassName)) {
                $(item).addClass(lastClassName);
            }
        }
    },

    /**
     * @name getTopLevelMenuItemCount
     * @description Get a count of top level menu items (Ignores submenu and their items)
     * @namespace sg.utls.OptionsMenu
     * @public
     * 
     * @return {number} Returns the count of top level menu items
     */
    getTopLevelMenuItemCount: function () {
        var count = 0;
        var menu = sg.utls.OptionsMenu.getOptionsMenu();
        if (menu) {
            var allMenuItems = $(menu).children();
            count = allMenuItems.length;
        }
        return count;
    },

    /**
     * Methods related to top-level MenuItems
     * Developer Note: MenuItems for SubMenus are handled in the SubMenu and SubMenu.MenuItem namespaces
     */
    MenuItem: {

        /**
         * @name add
         * @description Add a new item to the bottom of the OptionsMenu
         * @namespace sg.utls.OptionsMenu.MenuItem
         * @public
         * 
         * @param {string} menuItemId The ID of the new menu item
         * @param {string} label The label for the new menu item
         * @param {string} href An optional href specification
         */
        add: function (menuItemId, label, href = '') {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {

                // Find all menu items 'li' that contain a class called 'k-last' 
                var lastMenuItems = $(menu).find('.k-last');
                $.each(lastMenuItems, function (key, value) {

                    // if this li contains a class called 'menu-with-submenu', remove 'k-last' from class list
                    if (value.classList.contains('menu-with-submenu') || // submenu within main menu
                        (value.classList.contains('k-item') && !value.classList.contains('menu-active'))) { // Ignore items within submenu (of main menu)
                        $(value).removeClass('k-last');
                    }
                });

                // Add the new menu item
                var template = sg.utls.OptionsMenu.MenuItem._getDefaultTemplate(menuItemId, label, href);
                menu.append(template);

                // Ensure CSS classes are correct for each menu item
                sg.utls.OptionsMenu._fixCSS();
            }
        },

        /**
         * @name remove
         * @description Remove an existing menu item from the Options Menu
         * @namespace sg.utls.OptionsMenu.MenuItem
         * @public
         * 
         * @param {string} menuItemId The ID of the menu item to remove
         */
        remove: function (menuItemId) {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {
                var menuItemContainer = $(menu).find('#' + menuItemId);
                if (menuItemContainer) {
                    menuItemContainer.remove();
                }

                // Ensure CSS classes are correct for each menu item
                sg.utls.OptionsMenu._fixCSS();
            }
        },

        /**
         * @name setLabel
         * @description Set the visible label text for an existing menu item
         * @namespace sg.utls.OptionsMenu.MenuItem
         * @public
         * 
         * @param {string} menuItemId The ID of the menu item to update
         * @param {string} label The text label for the menu item
         */
        setLabel: function (menuItemId, label) {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {
                var menuItem = $(menu).find('#' + menuItemId);
                if (menuItem) {
                    menuItem.text(label);
                }
            }
        },

        /**
         * @name insertBefore
         * @description Insert a new menu item before an existing item
         * @namespace sg.utls.OptionsMenu.MenuItem
         * @public
         * 
         * @param {string} menuItemIdTarget The ID of the menu item were going to be inserting before
         * @param {string} menuItemId The ID of the new menu item
         * @param {string} label The label for the new menu item
         * @param {string} href An optional href specification
         */
        insertBefore: function (menuItemIdTarget, menuItemId, label, href = '') {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {

                // Build the menu item template
                var template = sg.utls.OptionsMenu.MenuItem._getDefaultTemplate(menuItemId, label, href);

                // Find the item were going to insert a new item before
                var targetMenuItem = $(menu).find('#' + menuItemIdTarget);
                if (targetMenuItem) {
                    // Insert the new menu item
                    var newMenuItem = $(template).insertBefore(targetMenuItem);
                }

                // Ensure CSS classes are correct for each menu item
                sg.utls.OptionsMenu._fixCSS();
            }
        },

        /**
         * @name insertAfter
         * @description Insert a new menu item after an existing item
         * @namespace sg.utls.OptionsMenu.MenuItem
         * @public
         * 
         * @param {string} menuItemIdTarget The ID of the menu item were going to be inserting after
         * @param {string} menuItemId The ID of the new menu item
         * @param {string} label The label for the new menu item
         * @param {string} href An optional href specification
         */
        insertAfter: function (menuItemIdTarget, menuItemId, label, href = '') {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {

                // Build the menu item template
                var template = sg.utls.OptionsMenu.MenuItem._getDefaultTemplate(menuItemId, label, href);

                // Find the item were going to insert a new item after
                var targetMenuItem = $(menu).find('#' + menuItemIdTarget);
                if (targetMenuItem) {
                    // Insert the new menu item
                    var newMenuItem = $(template).insertAfter(targetMenuItem);
                }

                // Ensure CSS classes are correct for each menu item
                sg.utls.OptionsMenu._fixCSS();
            }
        },

        /**
         * @name _getDefaultTemplate
         * @description Build the standard MenuItem template markup
         * @namespace sg.utls.OptionsMenu.MenuItem
         * @private
         * 
         * @param {string} menuItemId The ID of the menu item
         * @param {string} label The label for the new menu item
         * @param {string} href An optional href specification
         * @return {string} The markup for the new menu item
         */
        _getDefaultTemplate: function (menuItemId, label, href = '') {
            var hrefAttribute = '';
            if (href && href.length > 0) {
                hrefAttribute = " href='" + href + "' ";
            }
            var menuItemTemplate = `<li class='k-item k-state-default' role='menuitem' id='${menuItemId}'><a class='k-link k-menu-link'${hrefAttribute}>${label}</a></li>`;
            return menuItemTemplate;
        },
    },

    /**
     * Methods related to SubMenus
     */
    SubMenu: {

        /**
         * @name getItemCount
         * @description Get a count of the number of items in a SubMenu
         * @namespace sg.utls.OptionsMenu.SubMenu
         * @public
         *
         * @param {string} subMenuId The ID of the SubMenu to get item count of
         * @returns {number} The count of items in the SubMenu
         */
        getItemCount: function (subMenuId) {
            var count = 0;
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {
                var subMenuItems = $(menu).find('#' + subMenuId).children();
                count = subMenuItems.length;
            }
            return count;
        },

        /**
         * @name setLabel
         * @description Set the visible label text for an existing SubMenu
         * @namespace sg.utls.OptionsMenu.SubMenu
         * @public
         * 
         * @param {string} subMenuId The ID of the SubMenu to update
         * @param {string} label The text label for the SubMenu
         */
        setLabel: function (subMenuId, label) {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {
                var aTag = $(menu).find('#' + subMenuId).parent().find('a').first();
                if (aTag) {
                    aTag.text(label);
                }
            }
        },

        /**
         * @name add
         * @description Add a new SubMenu container and Optional MenuItems to the bottom of the OptionsMenu
         * @namespace sg.utls.OptionsMenu.SubMenu
         * @public
         * 
         * @param {string} subMenuId The ID of the new SubMenu (Refers to container)
         * @param {string} subMenuLabelId The ID of the new SubMenu label
         * @param {string} label The label for the new SubMenu
         * @param {object} items Optional object containing the new MenuItems to add to the SubMenu
         */
        add: function (subMenuId, subMenuLabelId, label, items = null) {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {

                // Find all menu items 'li' that contain a class called 'k-last' 
                var lastMenuItems = $(menu).find('.k-last');
                $.each(lastMenuItems, function (key, value) {

                    // if this li contains a class called 'menu-with-submenu', remove 'k-last' from class list
                    if (value.classList.contains('menu-with-submenu') || // submenu within main menu
                        (value.classList.contains('k-item') && !value.classList.contains('menu-active'))) { // Ignore items within submenu (of main menu)
                        $(value).removeClass('k-last');
                    }
                });

                // Add the new SubMenu and it's item container
                var template = sg.utls.OptionsMenu.SubMenu._getDefaultTemplate(subMenuId, subMenuLabelId, label);
                menu.append(template);

                // Get a reference to the SubMenu's item container and then add the items
                var subMenu = $(menu).find('#' + subMenuId);
                if (subMenu) {
                    sg.utls.OptionsMenu.SubMenu._insertItems(subMenu, items);
                }

                // Ensure CSS classes are correct for each menu item
                sg.utls.OptionsMenu._fixCSS();
            }
        },

        /**
         * @name remove
         * @description Remove an existing SubMenu and all of it's MenuItems
         * @namespace sg.utls.OptionsMenu.SubMenu
         * @public
         * 
         * @param {string} menuItemId The ID of the SubMenu to remove
         */
        remove: function (menuItemId) {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {
                var menuItemContainer = $(menu).find('#' + menuItemId).parent();
                if (menuItemContainer) {
                    menuItemContainer.remove();
                }

                // Ensure CSS classes are correct for each menu item
                sg.utls.OptionsMenu._fixCSS();
            }
        },

        /**
         * @name insertBefore
         * @description Insert a new SubMenu before an existing menu item
         * @namespace sg.utls.OptionsMenu.SubMenu
         * @public
         * 
         * @param {string} menuItemIdTarget The ID of the menu item were going to be inserting before
         * @param {string} subMenuId The ID of the new SubMenu
         * @param {string} subMenuLabelId The ID of the new SubMenu label
         * @param {string} label The label for the new SubMenu
         * @param {object} items Optional object containing the new MenuItems to add to the SubMenu
         */
        insertBefore: function (menuItemIdTarget, subMenuId, subMenuLabelId, label, items = null) {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {

                // Add the new SubMenu and it's item container
                var template = sg.utls.OptionsMenu.SubMenu._getDefaultTemplate(subMenuId, subMenuLabelId, label);

                // Find the item were going to insert a new item before
                var targetMenuItem = $(menu).find('#' + menuItemIdTarget);
                if (targetMenuItem) {
                    // Insert the new SubMenu
                    $(template).insertBefore(targetMenuItem);

                    // Get a reference to the SubMenu's item container
                    var subMenu = $(menu).find('#' + subMenuId);
                    if (subMenu) {
                        sg.utls.OptionsMenu.SubMenu._insertItems(subMenu, items);
                    }
                }

                // Ensure CSS classes are correct for each menu item
                sg.utls.OptionsMenu._fixCSS();
            }
        },

        /**
         * @name insertAfter
         * @description Insert a new SubMenu after an existing menu item
         * @namespace sg.utls.OptionsMenu.SubMenu
         * @public
         * 
         * @param {string} menuItemIdTarget The ID of the menu item were going to be inserting after
         * @param {string} subMenuId The ID of the new SubMenu
         * @param {string} subMenuLabelId The ID of the new SubMenu label
         * @param {string} label The label for the new SubMenu
         * @param {object} items Optional object containing the new MenuItems to add to the SubMenu
         */
        insertAfter: function (menuItemIdTarget, subMenuId, subMenuLabelId, label, items = null) {
            var menu = sg.utls.OptionsMenu.getOptionsMenu();
            if (menu) {

                // Add the new SubMenu and it's item container
                var template = sg.utls.OptionsMenu.SubMenu._getDefaultTemplate(subMenuId, subMenuLabelId, label);

                // Find the item were going to insert a new item before
                var targetMenuItem = $(menu).find('#' + menuItemIdTarget);
                if (targetMenuItem) {
                    // Insert the new SubMenu
                    $(template).insertAfter(targetMenuItem);

                    // Get a reference to the SubMenu's item container
                    var subMenu = $(menu).find('#' + subMenuId);
                    if (subMenu) {
                        sg.utls.OptionsMenu.SubMenu._insertItems(subMenu, items);
                    }
                }

                // Ensure CSS classes are correct for each menu item
                sg.utls.OptionsMenu._fixCSS();
            }
        },

        /**
         * @name _getDefaultTemplate
         * @description Build the standard SubMenu template markup
         * @namespace sg.utls.OptionsMenu.SubMenu
         * @private
         * 
         * @param {string} subMenuId The ID of the SubMenu (container below the label section)
         * @param {string} subMenuLabelId The ID of the SubMenu's label
         * @param {string} label The label for the new SubMenu
         * @return {string} The markup for the new SubMenu
         */
        _getDefaultTemplate: function (subMenuId, subMenuLabelId, label) {
            var subMenuTemplate = `<li class="menu-with-submenu k-item k-state-default k-last" aria-haspopup="true" role="menuitem">
                        <a id="${subMenuLabelId}" class="k-link k-menu-link">${label}<span class="k-icon k-i-arrow-60-right k-menu-expand-arrow"></span></a>
                        <ul class="sub-menu k-group k-menu-group" id="${subMenuId}" role="menu" aria-hidden="true"></ul>
                        </li>`;
            return subMenuTemplate;
        },

        /**
         * @name _insertItems
         * @description Insert new items into an existing SubMenu
         *              Developer Note: This method is meant to be called by the public methods
         *              add, insertBefore and insertAfter.
         * @namespace sg.utls.OptionsMenu.SubMenu
         * @private
         * 
         * @param {object} subMenu The existing SubMenu object
         * @param {object} items Object containing the new MenuItems to add to the SubMenu
         */
        _insertItems: function (subMenu, items) {

            // We're any items specified?
            if (items && items.length > 0) {
                $.each(items, function (key, value) {
                    var listItemId = value.listItemId;
                    var anchorId = value.anchorId;
                    var label = value.label;
                    var onClick = value.onClick;

                    var subMenuItemTemplate = sg.utls.OptionsMenu.SubMenu.MenuItem._getDefaultTemplate(listItemId, anchorId, label, onClick);

                    subMenu.append(subMenuItemTemplate);
                });
            }
        },

        /*
         * SubMenu - MenuItem related methods
         */
        MenuItem: {

            /**
             * @name _getDefaultTemplate
             * @description Build the standard SubMenu MenuItem template markup
             * @namespace sg.utls.OptionsMenu.SubMenu.MenuItem
             * @private
             * 
             * @param {string} menuItemId The ID of the menu item
             * @param {string} menuItemLabelId The ID of the menu item label
             * @param {string} label The label for the new menu item
             * @param {string} clickHandler An optional onClick handler specification
             * @return {string} The markup for the new menu item
             */
            _getDefaultTemplate: function (menuItemId, menuItemLabelId, label, clickHandler = '') {
                var clickHandlerAttribute = ' ';
                if (clickHandler && clickHandler.length > 0) {
                    clickHandlerAttribute = " onClick='" + clickHandler + "' ";
                }
                var template = `<li id="${menuItemId}" class="k-item k-state-default k-first" role="menuitem">
                            <a class="k-link k-menu-link" id="${menuItemLabelId}"${clickHandlerAttribute}>${label}</a></li>`;
                return template;
            },

            /**
             * @name add
             * @description Add a new MenuItem or MenuItems to an existing SubMenu
             * @namespace sg.utls.OptionsMenu.SubMenu.MenuItem
             * @public
             * 
             * @param {string} subMenuId The ID of the existing SubMenu
             * @param {object} items Object containing the new MenuItem(s) to add to the SubMenu
             */
            add: function (subMenuId, items) {
                var menu = sg.utls.OptionsMenu.getOptionsMenu();
                if (menu) {

                    // Get a reference to the SubMenu's item container and then add the items
                    var subMenu = $(menu).find('#' + subMenuId);
                    if (subMenu) {
                        sg.utls.OptionsMenu.SubMenu._insertItems(subMenu, items);
                    }

                    // Ensure CSS classes are correct for each menu item
                    sg.utls.OptionsMenu._fixCSS();
                }
            },

            /**
             * @name remove
             * @description Remove an existing MenuItem from an existing SubMenu
             * @namespace sg.utls.OptionsMenu.SubMenu.MenuItem
             * @public
             * 
             * @param {string} subMenuId The ID of the existing SubMenu
             * @param {string} menuItemId The ID of the MenuItem to remove
             */
            remove: function (subMenuId, menuItemId) {
                var menu = sg.utls.OptionsMenu.getOptionsMenu();
                if (menu) {
                    var menuItem = $(menu).find('#' + subMenuId).find('#' + menuItemId);
                    if (menuItem) {
                        menuItem.remove();
                    }

                    // Ensure CSS classes are correct for each menu item
                    sg.utls.OptionsMenu._fixCSS();
                }
            },

            /**
             * @name insertBefore
             * @description Insert a new MenuItem or MenuItems into an existing SubMenu before an existing MenuItem
             * @namespace sg.utls.OptionsMenu.SubMenu.MenuItem
             * @public
             * 
             * @param {string} subMenuId The ID of the existing SubMenu
             * @param {string} targetMenuItemId The ID of the existing MenuItem we will be inserting before
             * @param {object} items Object containing the new MenuItem(s) to add to the SubMenu
             */
            insertBefore: function (subMenuId, targetMenuItemId, items) {
                var menu = sg.utls.OptionsMenu.getOptionsMenu();
                if (menu) {

                    // Get a reference to the SubMenu's item container
                    var subMenu = $(menu).find('#' + subMenuId);
                    if (subMenu) {

                        var targetMenuItem = $(subMenu).find('#' + targetMenuItemId);

                        // We're any items specified?
                        if (items && items.length > 0) {
                            $.each(items, function (key, value) {
                                var listItemId = value.listItemId;
                                var anchorId = value.anchorId;
                                var label = value.label;
                                var onClick = value.onClick;

                                var subMenuItemTemplate = sg.utls.OptionsMenu.SubMenu.MenuItem._getDefaultTemplate(listItemId, anchorId, label, onClick);

                                targetMenuItem.before(subMenuItemTemplate);
                            });
                        }
                    }

                    // Ensure CSS classes are correct for each menu item
                    sg.utls.OptionsMenu._fixCSS();
                }
            },

            /**
             * @name insertAfter
             * @description Insert a new MenuItem or MenuItems into an existing SubMenu after an existing MenuItem
             * @namespace sg.utls.OptionsMenu.SubMenu.MenuItem
             * @public
             * 
             * @param {string} subMenuId The ID of the existing SubMenu
             * @param {string} targetMenuItemId The ID of the existing MenuItem we will be inserting after
             * @param {object} items Object containing the new MenuItem(s) to add to the SubMenu
             */
            insertAfter: function (subMenuId, targetMenuItemId, items) {
                var menu = sg.utls.OptionsMenu.getOptionsMenu();
                if (menu) {

                    // Get a reference to the SubMenu's item container
                    var subMenu = $(menu).find('#' + subMenuId);
                    if (subMenu) {

                        var targetMenuItem = $(subMenu).find('#' + targetMenuItemId);

                        // We're any items specified?
                        if (items && items.length > 0) {
                            $.each(items, function (key, value) {
                                var listItemId = value.listItemId;
                                var anchorId = value.anchorId;
                                var label = value.label;
                                var onClick = value.onClick;

                                var subMenuItemTemplate = sg.utls.OptionsMenu.SubMenu.MenuItem._getDefaultTemplate(listItemId, anchorId, label, onClick);

                                targetMenuItem.after(subMenuItemTemplate);
                            });
                        }
                    }

                    // Ensure CSS classes are correct for each menu item
                    sg.utls.OptionsMenu._fixCSS();
                }
            },
        },
    },
});
