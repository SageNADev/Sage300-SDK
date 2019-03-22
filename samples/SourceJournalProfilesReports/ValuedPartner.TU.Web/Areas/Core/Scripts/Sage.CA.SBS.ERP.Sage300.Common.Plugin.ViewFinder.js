﻿/* Copyright (c) 2019 Sage Software, Inc.  All rights reserved. */
"use strict";
(function (sg, $) {
    sg.viewFinderHelper = {
        pageSize: 5,
        cancelFuncCall: $.noop,

        /**
         * @name getFinderSettings
         * @description Get the settings for a finder based on ModuleID and ModuleAction
         * @public
         * @param {string} moduleId - The module Id as a string
         * @param {string} moduleAction - The action name as a string
         * @returns {object} Object representing the finder settings
         */
        getFinderSettings: function(moduleId, moduleAction) {
			return sg.viewFinderProperties[moduleId][moduleAction];
        },
        
        /**
         * @name initFinder
         * @desc Generic routine to initialize an individual finder
         * @private
         * @param {string} id - The id of the button used to invoke the finder
         * @param {any} parent - Dual purpose parameter 
         *                       1. Name of underlying control that will receive the
         *                          selected item
         *                       2. Callback when finder item selected                                              
         * @param {object} properties - Object containing various settings for the finder
         * @param {object} filter - The optional filter used to filter the finder results
         * @param {number} height - The optional height of the finder window
         * @param {number} top - The optional top location of the finder window
         */
        initFinder: function(id, parent, properties, filter, height, top) {

            let initFinder = function (viewFinder) {
                viewFinder.viewID = properties.viewID;
                viewFinder.viewOrder = properties.viewOrder;
                viewFinder.displayFieldNames = properties.displayFieldNames;
                viewFinder.returnFieldNames = properties.returnFieldNames;

                // Optional
                //     Only useful for UIs such as Invoice Entry finder where you 
                //     want to restrict the entries to a specific batch
                //
                // Note: 
                //     The following syntax is a workaround for IE which doesn't 
                //     support default parameters.
                //
                var filter = filter || null;
                viewFinder.filter = filter;
            };

            sg.viewFinderHelper.setViewFinder(id, parent, initFinder, null, height, top);
        },

        /**
         * Parameters for use in creating a View Finder Web screen finder widget.
         * 
         * @typedef {object} SetViewFinderProperties
         * @property {string} properties.viewID - The ID of the view the finder will use.
         * @property {number} properties.viewOrder - The view's sort-order field.
         * @property {string[]} properties.displayFieldNames - The names of the view - fields to display in the finder.
         * @property {string[]} properties.returnFieldNames - Array containing the list of field names for which to return values.
         * @property {string[]=} properties.initKeyValues - Optional initial key values for the finder. (If omitted, the starting value is blank.)
         * @property {string=} properties.filter  Optional filter string used for UIs such as Invoice Entry finder to restrict the finder entries displayed.
         * @property {string=} properties.optionalFieldBindings - True to automatically include optional fields in the search result and filter.
         * @property {boolean=} [properties.parentValAsInitKey = false] - True to take the initKeyValues from the parent control. (The finder’s key must be one field and the parent must be a control.)
         * @property {boolean} properties.calculatePageCount - Optional value to ... ?
         * @property {boolean} [properties.reinterpretInitKeyValues = true] - True to prepare the initKeyValues on the server, false to leave them as- is.
         */

        /**
         * Creates a View Finder Web screen finder widget.
         * 
         * @param {string} id - The finder control's id.
         * @param {(string|function)} parent - Either the id of the finder’s parent control (the control that receives the finder value), or 
         *     a callback function that will be invoked when user selects a record in the finder.
         * @param {SetViewFinderProperties} properties - Parameters with which to configure the finder.
         * @param {function=} onCancelCallback - An optional cancel-callback.
         * @param {number=} height - The optional height of the finder window.
         * @param {number=} top - The optional top location of the finder window.
         */
        setViewFinder: function (id, parent, properties, onCancelCallback, height, top) {
            $("#" + id).ViewFinder({
                properties: properties,
                parent: parent,
                cancel: onCancelCallback,
                height: height,  
                top: top
            });
        },
    };

    sg.findEvent = null;

    /* Required for  Preferences */
    sg.isPreferencesPostback = false;
    sg.finderOptions = null;
    sg.filterType = null;
    /*--------------------------*/

    sg.keys = null;
    sg.FinderDisplayType = {
        All: 0,
        Filter: 1,
        Grid: 2
    };

    sg.finderDataType = {
        Amount: "amount",
        Integer: "int",
        Number: "number",
        Date: "date",
        Time: "time",
        Text: "text",
        SmallInteger: "smallint",
        Boolean: "bool",
        Decimal: "decimal"
    };
    sg.finderOperator = {
        GreaterThan: 0,
        GreaterThanOrEqual: 1,
        LessThan: 2,
        LessThanOrEqual: 3,
        NotEqual: 4,
        Equal: 5,
        Like: 6,
        StartsWith: 7,
        Contains: 8
    };
    //Not to be used
    sg.delayVariables = {
        IsInProgress: false,
        RowData: [],
        ColumnName: "",
        TextData: "",
        TextBoxElement: ""
    };
    sg.delayOnBlur = function (elementId, funcCall) {
        var elementInfocus;
        setTimeout(function () {
            elementInfocus = $(document.activeElement);
            var elementInfocusAttrId = elementInfocus.attr("id");

            if (sg.utls.isSafari()) {
                elementInfocus = sg.findEvent;
                if (elementInfocus != null) {
                    elementInfocusAttrId = elementInfocus.id;
                } else {
                    elementInfocusAttrId = null;
                }
                sg.findEvent = null;
            }
            var isFound = false;
            if (elementId.constructor === Array) {
                isFound = $.inArray(elementInfocusAttrId, elementId) == -1;
            } else {
                isFound = elementInfocusAttrId != elementId;
            }
            if (elementInfocus == null || elementInfocusAttrId == null || isFound) {
                funcCall();
            } else {
                sg.viewFinderHelper.cancelFuncCall = funcCall;
            }
        });
    };
    sg.delayOnChange = function (elementId, txtElementId, funcCall) {
        var elementInfocus;
        setTimeout(function () {
            elementInfocus = $(document.activeElement);
            var elementInfocusAttrId = elementInfocus.attr("id");

            if (sg.utls.isSafari()) {
                elementInfocus = sg.findEvent;
                if (elementInfocus != null) {
                    elementInfocusAttrId = elementInfocus.id;
                } else {
                    elementInfocusAttrId = null;
                }
                sg.findEvent = null;
            }
            var isFound = false;
            if (elementId.constructor === Array) {
                isFound = $.inArray(elementInfocusAttrId, elementId) == -1;
            } else {
                isFound = elementInfocusAttrId != elementId;
            }
            if (elementInfocus == null || elementInfocusAttrId == null || isFound) {
                funcCall();
            }
            else {
                sg.delayVariables.IsInProgress = true;
                sg.delayVariables.TextBoxElement = txtElementId;
                sg.delayVariables.TextData = sg.delayVariables.TextBoxElement.text();
                sg.delayVariables.RowData = [];
                sg.delayVariables.ColumnName = "";
                sg.viewFinderHelper.cancelFuncCall = funcCall;
            }
        });
    };
    sg.delayOnDataChange = function (elementId, rowData, columnName, funcCall) {
        var elementInfocus;
        setTimeout(function () {
            elementInfocus = $(document.activeElement);
            var elementInfocusAttrId = elementInfocus.attr("id");

            if (sg.utls.isSafari()) {
                elementInfocus = sg.findEvent;
                if (elementInfocus != null) {
                    elementInfocusAttrId = elementInfocus.id;
                } else {
                    elementInfocusAttrId = null;
                }
                sg.findEvent = null;
            }

            if (elementInfocus == null || elementInfocusAttrId == null || elementInfocusAttrId != elementId) {
                funcCall();
            } else {
                sg.delayVariables.IsInProgress = true;
                sg.delayVariables.RowData = rowData;
                sg.delayVariables.ColumnName = columnName;
                sg.delayVariables.TextBoxElement = "";
                sg.delayVariables.TextData = "";
                sg.viewFinderHelper.cancelFuncCall = funcCall;
            }
        });

    };
}(this.sg = this.sg || {}, jQuery));

(function ($, window, document, undefined) {
    var kendoWindow;
    $.widget("sageuiwidgets.ViewFinder", {
        divFinderDialogId: '',
        options: {
            finderProperties: null,
            pageNumber: 1,
            pageSize: sg.viewFinderHelper.pageSize, 
            sortDir: false,
            select: $.noop,
            cancel: $.noop,
            title: "",
            id: "",
            postbackNotRequired: false
        },
        _create: function () {
            var that = this;
            $(that.element).bind('click', function () {

                if (!sg.utls.isFinderClicked) {
                    sg.utls.isFinderClicked = true;
                    sg.isPreferencesPostback = false;
                    that._doAjax(that);
                }
            });

            var isSafari = navigator.userAgent.indexOf("Safari") > -1;

            if (isSafari) {
                $(that.element).bind('mousedown', function () {

                    if (!sg.utls.isFinderClicked) {
                        sg.findEvent = event.currentTarget;
                    }
                });
            }
        },
        _doAjax: function (that) {

            if (typeof that.options.properties === 'function') {
                if (that.options.finderProperties === null) {
                    that.options.finderProperties = {};
                }

                that.options.properties(that.options.finderProperties);
            }
            else {
                that.options.finderProperties = that.options.properties;
            }

            var finderOptions = {
                ViewID: that.options.finderProperties.viewID,
                ViewOrder: that.options.finderProperties.viewOrder,
                DisplayFieldNames: that.options.finderProperties.displayFieldNames,
                ReturnFieldNames: that.options.finderProperties.returnFieldNames,
                InitKeyValues: that.options.finderProperties.initKeyValues,
                Filter: that.options.finderProperties.filter,
                ColumnFilter: null,  // no column filter initially
                PageNumber: that.options.pageNumber,
                PageSize: that.options.pageSize,
                OptionalFieldBindings: that.options.finderProperties.optionalFieldBindings,
                CalculatePageCount: true,
                InitialKeyFieldInDropdownList: that.options.finderProperties.initialKeyFieldInDropdownList,
                ReinterpretInitKeyValues: true
            };
            if (that.options.finderProperties.calculatePageCount != null)
                finderOptions.CalculatePageCount = that.options.finderProperties.calculatePageCount;

            if (that.options.finderProperties.reinterpretInitKeyValues != null)
                finderOptions.ReinterpretInitKeyValues = that.options.finderProperties.reinterpretInitKeyValues;

            // set the initial key values if caller asks so
            if (that.options.finderProperties.parentValAsInitKey !== null &&
                that.options.finderProperties.parentValAsInitKey &&
                that.options.parent !== 'function') {

                var ctrl = $("#" + that.options.parent);

                if (ctrl.hasClass("txt-upper")) {
                    finderOptions.InitKeyValues = [ctrl.val().toUpperCase()];
                }
                else {
                    finderOptions.InitKeyValues = [ctrl.val()];
                }
            }

            sg.finderOptions = finderOptions;

            // set the initial loading flag
            var data = { finderOptions: finderOptions, InitialLoading: true };

            that.divFinderDialogId = 'div_' + finderOptions.ViewID + '_dialog';
            $('<div id="' + that.divFinderDialogId + '"  style="display:none"/>').appendTo('body');
            var dialogId = "#" + that.divFinderDialogId;

            var finderWidth = 820;
            var sameOrigin = sg.utls.isSameOrigin();
            var finderLeftPos = (window.innerWidth - finderWidth) / 2;
            if (sameOrigin) {
                var activeWidgetConfigIframe = $(window.top.$('iframe[id^="iframeWidgetConfiguration"]:visible'));
                if (activeWidgetConfigIframe.is(':visible')) {
                    finderLeftPos = (activeWidgetConfigIframe.parents('.k-widget.k-window').width() - finderWidth) / 2;
                }
            }

            var finderHeight = 552;
            if (that.options.height !== undefined && that.options.height !== null && typeof that.options.height === 'number') {
                finderHeight = that.options.height;
            }

            var top = that.options.top;

            kendoWindow = $(dialogId).html("<span class='sage_loading'></span>").kendoWindow({
                modal: true,
                title: that.options.title,
                resizable: false,
                draggable: true,
                scrollable: false,
                visible: false,
                navigatable: true,
                width: finderWidth,
                height: finderHeight,
                activate: sg.utls.kndoUI.onActivate,
                //Open Kendo Window in center of the Viewport
                open: function () {
                    var sameOrigin = sg.utls.isSameOrigin();
                    if (sameOrigin) {
                        var portalHeight = window.top.sg.utls.portalHeight;
                        var windowHeight = $(window.top).scrollTop() - portalHeight;
                        var finderTopPos = (($(window.top).height() - kendoWindow.options.height) / 2) + windowHeight;
                        if (finderTopPos < 0) {
                            finderTopPos = 0;
                        }
                        if (top) {
                            finderTopPos = top;
                        }
                    } else {
                        finderTopPos = 20;
                    }

                    this.wrapper.css({ top: finderTopPos });
                    this.wrapper.css({ left: finderLeftPos });
                },
            }).data("kendoWindow");

            //Close Event -Do same as cancel
            kendoWindow.bind("close", function () {
                that._triggerChange(that);
                kendoWindow.destroy();
                var cancel = that.options.cancel;
                if (cancel) {
                    cancel();
                }
                sg.utls.isFinderClicked = false;
                sg.findEvent = null;
            });

            $(dialogId).parent().addClass("finder-window");
            window.sg.utls.ajaxPostHtml(window.sg.utls.url.buildUrl("Core", "ViewFinder", "Find"), data, function (successData) {
                that._showFinderScreen(that, successData, dialogId);
            }, function (jqXhr, textStatus, errorThrown) {
                sg.utls.isFinderClicked = false;
                sg.utls.ajaxErrorHandler(jqXhr, textStatus, errorThrown);
            });
        },
        _showFinderScreen: function (that, data, dialogId) {
            if (data) {
                $(dialogId).html(data);

                sg.finderOptions.ColumnPreferences = columnPreferences;
                ViewFinderGridHelper.init(sg.finderOptions);

                // if there is initial column filter, show it
                ViewFinderGridHelper.InitFinderValues(columnFilter);
                sg.finderOptions.ColumnFilter = columnFilter;

                FinderPreferences.Initialize();
                var $titleSpan = kendoWindow.wrapper.find('.k-window-title');
                $titleSpan.html(finderTitle);
                kendoWindow.open();

                $(document)
                    .on('click.plugin.finderPref',
                        "#btnFinderPrefApply",
                        function() {
                            sg.isPreferencesPostback = true;
                            that._reload(that, false);
                        });

                $(document)
                    .on('click.plugin.finderPref',
                        "#btnFinderPrefRestore",
                        function() {
                            sg.isPreferencesPostback = true;
                            that._reload(that, true);
                        });

                $(document)
                    .on('click.plugin.finderPref',
                        "#btnFinderPrefEditCols",
                        function() {
                            var prefHtml = $("#tblTBodyFinderPref").html();
                            if (prefHtml !== "") {
                                FinderPreferences.ShowFieldsWindow();
                            } else {
                                var data = {finderOptions:sg.finderOptions};
                                window.sg.utls.ajaxPostHtmlSync(window.sg.utls.url
                                    .buildUrl("Core", "ViewFinder", "GetEditableColumns"),
                                    data,
                                    function(successData) {
                                        $("#tblTBodyFinderPref").html(successData);
                                        FinderPreferences.FinderPreferencesHTML = $("#tblTBodyFinderPref").html();
                                        FinderPreferences.ShowFieldsWindow();
                                    });
                            }
                        });

                $("#select")
                    .on('click',
                        function() {
                            sg.viewFinderHelper.cancelFuncCall = $.noop;
                            sg.delayVariables.IsInProgress = false;
                            that._getSelectedRow(that);
                        });

                $("#cancel")
                    .on('click',
                        function() {
                            that._triggerChange(that);
                            var cancel = that.options.cancel;
                            if (cancel) {
                                $(this).on('click', cancel());
                            }
                            var finderWin = $("#" + that.divFinderDialogId).data("kendoWindow");
                            finderWin.destroy();
                            sg.utls.isFinderClicked = false;
                            sg.findEvent = null;

                        });
                $("#div_finder_grid .k-grid-content")
                    .delegate("tbody>tr",
                        "dblclick",
                        function() {
                            sg.viewFinderHelper.cancelFuncCall = $.noop;
                            that._getSelectedRow(that);
                        });
            } else {
                kendoWindow.destroy()
                sg.utls.isFinderClicked = false;
            }
        },

        _reload: function (that, deleteUserPreference) {
            var options = sg.finderOptions;
            options.PageNumber = 1;
            options.CanSavePreferences = options.CanDeletePreferences = false;

            if (deleteUserPreference) {
                options.CanDeletePreferences = true;
            } else {
                options.CanSavePreferences = true;
                options.ColumnPreferences = FinderPreferences.GetSelectedColumns();
            }
            that._reloadFinder(that, options);
        },

        _reloadFinder: function (that, options) {
            var data = { finderOptions: options };
            window.sg.utls.ajaxPostHtml(window.sg.utls.url.buildUrl("Core", "ViewFinder", "Find"), data, function (successData) {
                that._showFinderScreen(that, successData, "#" + that.divFinderDialogId);
            });
        },

        _triggerChange: function (that) {
            that._resetFocus(that);
            if (sg.delayVariables.IsInProgress) {
                sg.delayVariables.IsInProgress = false;
                if (sg.delayVariables.RowData.Length > 0) {
                    var data = sg.delayVariables.RowData[sg.delayVariables.ColumnName];
                    sg.delayVariables.RowData.set(sg.delayVariables.ColumnName, "");
                    sg.delayVariables.RowData.set(sg.delayVariables.ColumnName, data);
                }
                if (sg.delayVariables.TextBoxElement) {
                    sg.delayVariables.TextBoxElement.change();
                }
            }
            if (sg.viewFinderHelper.cancelFuncCall != $.noop) {
                sg.viewFinderHelper.cancelFuncCall();
                sg.viewFinderHelper.cancelFuncCall = $.noop;
            }
        },

        _selectGrid: function (that) {
            var grid, row, data, retObject;
            if ($('#div_finder_grid')) {
                grid = $('#div_finder_grid').data("kendoGrid");
                row = grid.select();
                if (row.length !== 0) {
                    data = grid.dataItem(row);
                    retObject = {};
                    for (var i = 0; i < sg.finderOptions.ReturnFieldNames.length; i++) {
                        var cellVal = data[sg.finderOptions.ReturnFieldNames[i]];
                        var column = $.grep(ViewFinderGridHelper.columns, function (column) {
                            return column.field === sg.finderOptions.ReturnFieldNames[i];
                        });

                        var val;
                        if (column.length === 1) {
                            // check data type
                            if (column[0].PresentationList !== null) {
                                var pval = $.grep(column[0].PresentationList, function (p) {
                                    return p.Text === cellVal;
                                });

                                if (pval.length === 1) {
                                    val = pval[0].Value;
                                }
                            }
                            else if (column[0].dataType === sg.finderDataType.Date) {
                                val = sg.utls.kndoUI.getFormattedDate(cellVal);
                            }
                            else {
                                val = cellVal;
                            }

                            retObject[sg.finderOptions.ReturnFieldNames[i]] = val;
                        }
                    }
                }
            }
            return retObject;
        },

        _isNullOrUndefined: function (variable) {
            if (variable === null || typeof variable === "undefined") {
                return false;
            }

            return true;
        },

        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        },

        _getSelectedRow: function (that) {
            that._resetFocus(that);
            var dataSelected = that._selectGrid(that);
            if (that._isNullOrUndefined(dataSelected)) {
                $('.selectKendoGrid').attr("disabled", false).removeClass("btnStyle2Disabled");

                if (typeof that.options.parent === 'function') {
                    that.options.parent(dataSelected);
                }
                else {
                    $("#" + that.options.parent).val(dataSelected[Object.keys(dataSelected)[0]]);
                    $("#" + that.options.parent).trigger("change");
                    $("#" + that.options.parent).trigger("blur");
                }

                var finderWin = $("#" + that.divFinderDialogId).data("kendoWindow");
                if (finderWin !== undefined) {
                    finderWin.destroy();
                    sg.utls.isFinderClicked = false;
                    sg.findEvent = null;
                }
            }
        },
        _resetFocus: function (that) {
            var finderElement = $("#" + that.options.id);
            if (finderElement.length === 0) {
                finderElement.focus();
            } else {
                finderElement[0].focus();
            }
        }
    });

})(jQuery, window, document);

/* Finder Grid Columns Customization */
var FinderPreferences = FinderPreferences || {};
FinderPreferences = {
    FinderPreferencesHTML: null,

    Initialize: function () {
        $("#tblTBodyFinderPref").sortable({ cursor: "move" }).disableSelection();

        $(document).off('.plugin.finderPref');

        $(document).on('click.plugin.finderPref', "#btnFinderPrefCancel", function () {
            $("#tblTBodyFinderPref").html(FinderPreferences.FinderPreferencesHTML);
            FinderPreferences.Hide();
        });

        $(document).on('click.plugin.finderPref', "#chkSelectAll", function () {
            FinderPreferences.ChangeCheckState(this.checked);
        });
        $(document).on('click.plugin.finderPref', "[name='chkFinderPrefCol']", function () {
            FinderPreferences.SelectHeaderCheckBox();
        });
        $(document).on('click.plugin.finderPref', function (e) {
            var container = $('#divFinderPrefEditCols');
            var editColumnButton = $("#btnFinderPrefEditCols");
            // if the target of the click isn't the container... nor a descendant of the container
            if (!container.is(e.target) && !editColumnButton.is(e.target) && container.has(e.target).length === 0) {
                container.hide();
            }
        });
    },

    // Check/Uncheck the "Select All" checkbox based on the selection of list of columns
    SelectHeaderCheckBox: function () {
        $('#chkSelectAll').attr('checked', 'checked').applyCheckboxStyle();
        $('#tblTBodyFinderPref input[name=chkFinderPrefCol][type=checkbox]').each(function () {
            if (!$(this).is(':checked')) {
                $('#chkSelectAll').removeAttr('checked').applyCheckboxStyle();
                return;
            }
        });
    },
    /**
     * Hide the list of columns
     * @method hide      
     */
    Hide: function () {
        $('#divFinderPrefEditCols').hide();
    },
    // Show or Hide list of columns
    ShowFieldsWindow: function () {
        FinderPreferences.CheckFinderPrefCols('#div_finder_grid');
        FinderPreferences.SelectHeaderCheckBox();
        var container = $('#divFinderPrefEditCols');
        var btnEdit = $('#btnFinderPrefEditCols');

        //Static position of edit grid.
        var topAlignment = 130;
        var leftAlignment = 10;

        $('#divFinderPrefEditCols').css({ top: topAlignment, left: leftAlignment, position: 'absolute', "z-index": "1000" });
        $('#divFinderPrefEditCols').show();
    },

    GetSelectedColumns: function () {
        var selectedColumns = [];
        $('#tblTBodyFinderPref input[name=chkFinderPrefCol][type=checkbox]').each(function () {
            if ($(this).is(':checkbox')) {
                selectedColumns.push(FinderPreferences.GetGridColumn($(this).attr('data-finder-key'), $(this).attr('value'), $(this).is(':checked')));
            }
        });
        return selectedColumns;
    },

    GetGridColumn: function (id, title, isChecked) {
        var gridColumn = new Object();
        gridColumn.field = id;
        gridColumn.desc = title;
        gridColumn.isHidden = !isChecked;
        return gridColumn;
    },

    // Check the columns by skipping saved hidden columns
    CheckFinderPrefCols: function (gridname) {
        var grid = $(gridname).data('kendoGrid');
        var isChecked = false;
        if (grid != null) {
            $('#tblTBodyFinderPref input[name=chkFinderPrefCol][type=checkbox]').each(function () {
                isChecked = false;
                for (var i = 0; i < grid.columns.length; i++) {
                    if ($(this).attr('data-finder-key') === grid.columns[i].field) {
                        if (!grid.columns[i].hidden) {
                            $(this).attr('checked', 'checked').applyCheckboxStyle();
                            isChecked = true;
                        }
                    }
                }
                if (!isChecked) {
                    $(this).removeAttr('checked').applyCheckboxStyle();
                }
            });
        }
    },

    // Select/Unselect all checkbox fields
    ChangeCheckState: function (flag) {
        $('#tblTBodyFinderPref input[name=chkFinderPrefCol][type=checkbox]').each(function () {
            if (flag) {
                $(this).attr('checked', 'checked').applyCheckboxStyle();
            } else {
                $(this).removeAttr('checked').applyCheckboxStyle();
            }

        });
    }
};
