/* Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved. */

/* global kendo */
/* exported InquiryGeneralUI */
/* global InquiryGeneralViewModel */
/* exported savedQueryResources */
/* global inquiryGeneralResources */

/// <summary>Kendo UI Web Filter Panel Plugin.</summary>
/// <description>Display the filter settings applied to a kendo.data.DataSource.</description>

(function ($) {
    var kendo = window.kendo, ui = kendo.ui, Widget = ui.Widget;

    var GridFilterPanel = Widget.extend({
        dataSource: null,

        options: {
            name: "GridFilterPanel",
            template: "<span style='color:blue'>" + inquiryGeneralResources.Filter + "</span> #= filter #",
            placeholder: "<span style='color:blue'>" + inquiryGeneralResources.NoFilter + "</span>",
            columns: []
        },

        init: function (element, options) {
            Widget.fn.init.call(this, element, options);
            this.element.attr("class", "k-block k-header");
            this.element.attr("style", "min-height:1.5em;");

            if (typeof this.options.dataSource !== "undefined") {
                this.setDataSource(this.options.dataSource);
            }
            this.element.append("<span class='k-message' style='display:inline-block; margin:0.4em;'></span>");
            this.displayFilter();
        },

        operatorText: function (filter) {
            var operatorText = (filter.operator ? filter.operator : filter.Operator);
            switch (operatorText) {
                case "eq":
                    operatorText = "=";
                    break;
                case "neq":
                    operatorText = "!=";
                    break;
                case "gte":
                    operatorText = ">=";
                    break;
                case "gt":
                    operatorText = ">";
                    break;
                case "lte":
                    operatorText = "<=";
                    break;
                case "lt":
                    operatorText = "<";
                    break;
            }
            //return "<span style='color:blue'>" + operatorText + "</span>";
            return operatorText;
        },

        getDisplayFilter: function (filter) {
            var filterText = "";
            var filters = filter.filters;
            if (filters == null) return filterText;
            var length = filters.length;
            var numberType = ["int32", "int64", "int16", "long", "byte", "real", "decimal"];
            for (var idx = 0; idx < length; idx++) {
                var child = filters[idx];
                if (child.filters != null && child.filters.length > 0) {
                    filterText += kendo.format("({0})", this.getDisplayFilter(child));
                } else {
                    var field = this.options.columns.filter(function (c) { return c.columnName === child.field; })[0];
                    if (!field) {
                        continue;
                    }

                    var type = field.dataType.toLowerCase();
                    var isNumericType = numberType.indexOf(type) > -1;

                    // datetime
                    var value = (type === "datetime") ? kendo.toString(kendo.parseDate(child.value), "d") : child.value; 

                    // handle where clause in query
                    value = value.replace(/'/g, '');

                    // enumerations
                    if (field.presentation) {
                        value = field.presentation.filter(function (item) { return item.Value == value; })[0].Text;
                    }

                    // numbers
                    if (isNumericType) {
                        value = kendo.toString(kendo.parseFloat(value, "en-US"), "n" + field.precision);
                    } 

                    // encode strings
                    value = (type === "string") ? kendo.htmlEncode(value) : value;

                    var format = isNumericType ? "{0} {1} {2}" : "{0} {1} '{2}'";
                    var title = field.title;
                    filterText += kendo.format(format, title, this.operatorText(child), value);
                }

                if (idx < (length - 1)) {
                    var opt = filter.logic.toLowerCase();
                    filterText += kendo.format((opt == 'and') || (opt == 'or') ? " <span>{0}</span> " : " {0} ", filter.logic);
                }
            }

            return filterText;
        },

        displayFilter: function () {
            var filterText = "";
            if (this.dataSource !== null && this.dataSource._filter) {
                var filter = this.dataSource._filter;
                filterText = this.getDisplayFilter(filter);
            }
            var htmlString = (filterText) ? kendo.template(this.options.template)({ filter: filterText }) : this.options.placeholder;
            this.element.find(".k-message").html(htmlString);
        },

        setDataSource: function (ds) {
            var that = this,
                change = function (e) {
                    that.displayFilter.call(that, e);
                };

            if (this.dataSource !== null) {
                this.dataSource.unbind("change", change);
            }
            this.dataSource = ds;
            this.dataSource.bind("change", change);
            this.displayFilter();
        }
    });

    ui.plugin(GridFilterPanel);

})(jQuery);

var InquiryGeneralUI = function () {
    // The culture is usually set in Global.js but there seems to be a timing issue so set it here first
    kendo.culture(globalResource.Culture);
    //var drillDowntemplate = '<div class="pencil-wrapper"><span class="pencil-txt">#= {0} #</span><span class="pencil-icon"><input type="button" class="icon edit-field inqueryEditCell"/></span></div>';
    var datetimeTemplate = '#if({0} === null || {0} == ""){##}else{# #= kendo.toString(kendo.parseDate({0}), "d") #  #}#';
    var filterOperator = { eq: "=", neq: "!=", gt: ">", gte: ">=", lt: "<", lte: "<=" };
    var stringFilterOperator = $.extend({}, filterOperator, { Like: "Like", StartsWith: "Starts With", Contains: "Contains" });
    var grid, pageSize = 10, dropdownDataSource = {};
    var btnLink1 = $("#btnPageSize10");
    var btnLink2 = $("#btnPageSize20");
    var btnLink3 = $("#btnPageSize30");
    var aggregates = [];
    var aggregateFields = [];
    var previousGroupName = "";
    var aggOptionShow = false;
    var savedInquiryTitle = "";

    initInquiryGeneralUI();

    //Init Inquiry General UI
    function initInquiryGeneralUI() {
        var userMessage = InquiryGeneralViewModel.UserMessage;
        if (userMessage && userMessage.Errors && userMessage.Errors.length > 0) {
            sg.utls.showMessage(InquiryGeneralViewModel);
            return;
        }
        //var viewName = InquiryGeneralViewModel.ViewName;
        showElements(InquiryGeneralViewModel);
        setHeaderTitle();
        initButtons();
        configGrid(null);

        //Fix cross domain issue. For cross domain, we can't access window parent from iframe'
        if (InquiryGeneralViewModel.IsAdhocQuery) {
            window.parent.addEventListener('message', function (event) {
                if (event.data.event_id === 'SaveQueryPanel') {
                    saveQueryPanel(event.data.template);
                }
            });
        }

        function saveQueryPanel(template)
        {
            template.TemplateId = InquiryGeneralViewModel.TemplateId;
            if (template.TemplateId === template.winTemplateId) {
                var grid = $("#inquiryGrid").data("kendoGrid");
                var columns = grid.getOptions().columns;
                var fields = columns.map(function (f) { f.IsDisplayable = !f.hidden; return f; });
                var filter = grid.getOptions().dataSource.filter;

                // Convert all dates in filter to a culture invariant format
                if (filter) {
                    var datetimefields = fields.filter(function (f) { return f.dataType.toLowerCase() === "datetime"; }); // cache datetime fields for better performance
                    filter = convertToCultureInvariantDate(filter, datetimefields);
                }

                var group = grid.getOptions().dataSource.group;

                savedInquiryTitle = template.Name;
                if (template.Type === inquiryGeneralResources.Private) {
                    template.Type = "Private";
                } else if (template.Type === inquiryGeneralResources.Public) {
                    template.Type = "Public";
                }

                var value = {
                    template: template,
                    fields: fields,
                    filter: filter,
                    group: group
                };
                var url = sg.utls.url.buildUrl("Core", "InquiryGeneral", "SaveTemplate");
                sg.utls.ajaxPost(url, value, saveTemplate);
            }
        }
    }

    // Recursive helper function to convert dates in a filter to a culture invariant format
    function convertToCultureInvariantDate(filter, datetimefields) {
        if (filter.filters && filter.filters.length > 0) {
            filter.filters.forEach(function(f) {
                convertToCultureInvariantDate(f, datetimefields); // recursive case
            });
        } else {
            // Check that the column being filtered is of datetime type
            for (var i = 0; i < datetimefields.length; i++) {
                if (datetimefields[i].field === filter.field) {
                    filter.value = kendo.toString(kendo.parseDate(filter.value), "yyyyMMdd"); // base case - do conversion
                    break;
                }
            }
        }      
        return filter;
    }

    //Show elements for adhoc inquiry
    function showElements(viewModel)
    {
        if (viewModel.IsAdhocQuery) {
            $("#divShowGrandTotals").show();
            $("#headerButtonsId").show();
        }
    }

    //Set header title based on Json file name
    function setHeaderTitle() {
        var title = InquiryGeneralViewModel.Title;
        $("#InquiryGeneralHeader").text(inquiryGeneralResources.Inquiry + " - " + title);
    }
    
    function saveTemplate(jsonResult) {
        if (jsonResult) {
            if (jsonResult.Errors) {
                sg.utls.showMessage(jsonResult);
            } else {
                InquiryGeneralViewModel.TemplateId = jsonResult.TemplateId;
                InquiryGeneralViewModel.Title = savedInquiryTitle;
                InquiryGeneralViewModel.Name = InquiryGeneralViewModel.Title;
                $("#InquiryGeneralHeader").text("Inquiry - " + savedInquiryTitle);
                $("#btnDeleteQuery").prop("disabled", false);
                window.parent.postMessage({ event_id: 'SaveTemplate', inquiryTitle: "Inquiry - " + savedInquiryTitle }, '*');
            }
        }
    }

    function exportDownload(jsonResult) {
       if (jsonResult) {
          if (jsonResult.Errors) {
             sg.utls.showMessage(jsonResult);
          } else {
             var filename = InquiryGeneralViewModel.Title.replace(/[/\\?%*:|"<>]/g, ''); //Strip illegal file/path name characters
             window.location = sg.utls.url.buildUrl("Core", "InquiryGeneral", "DownloadExport") + '?fileId=' + jsonResult.FileId + '&filename=' + filename;
          }
       }
    }

    function deleteTemplate(jsonResult) {
        if (jsonResult.Errors) {
            sg.utls.showMessage(jsonResult);
        } else {
            $("#btnDeleteQuery").prop("disabled", true);
            $("#btnSaveQuery").prop("disabled", true);
            //$("#btnOpenQuery").prop("disabled", true);
            $("#btnExportQuery").prop("disabled", true);
            window.parent.postMessage({ event_id: 'DeleteTemplate' }, '*');
        }
    }

    //Config inquiry grid columns
    function getGridColumns() {
        var items = InquiryGeneralViewModel.Fields;
        var length = items.length;
        var cols = [];
        var numbers = ["int32", "int64", "int16", "integer", "long", "byte", "real", "decimal"];
        var groupable = InquiryGeneralViewModel.IsAdhocQuery;

        for (var i = 0; i < length; i++) {
            var item = items[i];
            if (!item.Included) {
                continue;
            }
            var col = {},  type = item.DataType.toLowerCase();
            var classGroupable = (groupable && item.IsGroupable) ? "groupable" : "";
            var attr = (numbers.indexOf(type) > -1) ? "align-right " : "align-left ";
            var headerType = (numbers.indexOf(type) > -1) ?  "number" : type;
            var list = item.PresentationList;
            headerType = (list && list.length > 0) ? "enum": headerType;

            attr += classGroupable;
            if (groupable && (!aggOptionShow) && (item.IsDisplayable)) {
                aggOptionShow = true;
                col.locked = true;
                setTemplateCaption(col, "footer");
                setTemplateCaption(col, "group");
            }
            col.lockable = false;
            col.title = item.Title;
            col.field = item.FieldAlias || item.Field;
            col.isDummy = item.IsDummy;
            col.dataType = item.DataType;
            col.precision = item.Precision;
            col.isFilterable = item.IsFilterable;
            col.columnName = item.FieldAlias || item.Field;
            col.presentation = (list && list.length == 0) ? null : list;
            col.width = (item.IsDummy)? 100: 160;
            col.headerWidth = (item.IsDummy) ? 100 : 160;
            col.attributes = { "class": attr };
            col.headerAttributes = { "class": attr, "dataType": headerType };
            col.template = getTemplate(item);
            col.isDrillDown = item.IsDrilldown;
            col.drillDownUrl = item.DrilldownUrl;
            col.hidden = (item.IsDisplayable === undefined) ? false : !item.IsDisplayable;
            col.groupable = item.IsGroupable;
            col.isAggregate = item.IsAggregate;

            if (type === "string") {
                col.filterable = {
                    operators: {
                        string: stringFilterOperator
                    }
                };
            } else if (type === "datetime") {
                col.filterable = {
                    ui: "datepicker"
                };
            } else if (numbers.indexOf(type) > -1) {
                col.filterable = {
                    ui: $.proxy(numberFilter, { field: item })
                };
            } else if (list && list.length > 0) {
                dropdownDataSource[item.Field] = list;
                col.filterable = {
                    ui: $.proxy(dropdownFilter, { field: item.Field }),
                    operators: {
                        string: {
                            eq: "=",
                            neq: "!="
                        }
                    }
                };
            }
            //add aggregates for adhoc inquiry
            if (groupable)
            {
                var funcs = ['sum', 'min', 'max', 'avg', 'count'];
                // for all aggregates
                if (type === "decimal" && item.IsAggregate) {
                    funcs.forEach(function (key) {
                        aggregates.push({ field: col.field, aggregate: key });
                    });
                    if (aggregateFields.indexOf(col.field) < 0 ) {
                        aggregateFields.push(col.field);
                    }
                }
                //for group aggregates
                col.aggregates = item.Aggregates;
                if ( item.IsAggregate && item.Aggregates && item.Aggregates.length > 0) {
                    var aggrs = item.Aggregates.map(function (i) { return i.toLowerCase();});
                    var template = "";
                    for (var j = 0; j < 5; j++) {
                        if (aggrs.indexOf(funcs[j]) > -1) {
                            template += kendo.format("<div>#=kendo.toString({0},'n{1}')#</div>", funcs[j], (j === 4) ? 0 : 3);
                        }
                    }
                    col.groupFooterTemplate = template;
                }
                if (type === "decimal" & item.IsAggregate) {
                    col.footerTemplate = template;
                }
            }
            cols.push(col);
        }
        return cols;
    }

    function setTemplateCaption(col, mode)
    {
        var template = kendo.format("<span id=footer{0} ><div>{1}:</div>", col.field, (mode == "footer") ? inquiryGeneralResources.GrandTotal: inquiryGeneralResources.Total);
        template += kendo.format("<div>{0}:</div>", inquiryGeneralResources.Minimum);
        template += kendo.format("<div>{0}:</div>", inquiryGeneralResources.Maximum);
        template += kendo.format("<div>{0}:</div>", inquiryGeneralResources.Average);
        template += kendo.format("<div>{0}:</div></span>", inquiryGeneralResources.Count);
        (mode == "footer") ? col.footerTemplate = template : col.groupFooterTemplate = template;
    }

    function numberFilter(element) {
        element.kendoNumericTextBox({
            format: "n" + this.field.Precision,
            spinners: false,
            decimals: this.field.Precision
        });
    }

    //DropDown list for column filter
    function dropdownFilter(element) {
        element.kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: dropdownDataSource[this.field]
        });
    }

    function getOperator(filter) {
        var operatorText = (filter.operator ? filter.operator: filter.Operator) ;
        switch (operatorText) {
            case "eq":
                operatorText = "=";
                break;
            case "neq":
                operatorText = "!="; 
                break;
            case "gte":
                operatorText = ">=";
                break;
            case "gt":
                operatorText = ">";
                break;
            case "lte":
                operatorText = "<=";
                break;
            case "lt":
                operatorText = "<";
                break;
            case "Like":
            case "StartsWith":
            case "Contains":
                operatorText = "LIKE";
                break;
        }
        return operatorText;
    }

    //Get filter string
    function getFilterString(filter) {
        var filterString = "";
        if (!filter) return filterString;

        var filters = filter.filters;
        if (filters == null) return filterString;
        var length = filters.length;
        var numberType = ["Int32", "Int64", "Int16", "Long", "Byte", "Real", "Decimal"];
        var operators = ["like", "startswith", "contains", "endwith"];

        for (var idx = 0; idx < length; idx++) {
            var child = filters[idx];
            if (child.filters != null && child.filters.length > 0 ) {
                filterString += kendo.format("({0})", getFilterString(child));
            } else {
                var cols = (grid === undefined) ? getGridColumns() : grid.columns;
                var field = cols.filter(function (c) { return c.columnName == child.field; })[0];
                if (!field) {
                    continue;
                }
                var fieldName = child.field;
                var type = field.dataType.toLowerCase();

                var value;
                if (type === "datetime") {
                    var dateObj = kendo.parseDate(new Date(child.value));
                    value = kendo.toString(dateObj, "yyyyMMdd");
                    child.value = kendo.toString(dateObj, 'd');
                } else {
                    value = child.value;
                }
                
                var isSql = "";

                var operator = (child.operator ? child.operator : child.Operator).toLowerCase();

                if (operators.indexOf(operator) > -1) {
                    var expression = "", sFormat = "";
                    isSql = InquiryGeneralViewModel.Sql;
                    value = (isSql) ? value.toUpperCase() : value;
                    if (operator === "like") {
                        sFormat = (isSql) ? "upper({0}) LIKE '%{1}%'" : "{0} LIKE %{1}%";
                        var sFormat0 = (isSql) ? "upper({0}) LIKE '{1}'" : "{0} LIKE {1}";
                        expression = (value.indexOf("%") > -1) ? kendo.format(sFormat0, fieldName, value) : kendo.format(sFormat, fieldName, value);
                    }
                    if (operator === "contains") {
                        sFormat = (isSql) ? "upper({0}) LIKE '%{1}%'" : "{0} LIKE %{1}%";
                        expression = kendo.format(sFormat, fieldName, value);
                    }
                    if (operator === "startswith") {
                        sFormat = (isSql) ? "upper({0}) LIKE '{1}%'" : "{0} LIKE {1}%";
                        expression = kendo.format(sFormat, fieldName, value);
                    }
                    if (operator === "endwith") {
                        sFormat = (isSql) ? "upper({0}) LIKE '%{1}'" : "{0} LIKE %{1}";
                        expression = kendo.format(sFormat, fieldName, value);
                    }
                    filterString += expression;
                } else {
                    isSql = InquiryGeneralViewModel.Sql;
                    //var format = numberType.indexOf(field.dataType) > -1 ? "{0} {1} {2}" : (isSql) ? "{0} {1} '{2}'" : "{0} {1} \"{2}\"";
                    var format = "";
                    var caseInsensitive = (field.dataType.toLowerCase() == 'string' && operator == 'eq');
                    value = (caseInsensitive && isSql) ? value.toUpperCase() : value;

                    if (numberType.indexOf(field.dataType) > -1) {
                        var format = "{0} {1} {2}";
                    } else {
                        if (isSql) {
                            if (value.charAt(0) == "'" && value.charAt(value.length - 1) == "'") {
                                format = "{0} {1} {2}";
                            } else {
                                format = (caseInsensitive) ? "upper({0}) {1} '{2}'" : "{0} {1} '{2}'";
                            }
                        } else {
                            format = "{0} {1} \"{2}\"";
                        }
                    }
                    operator = getOperator(child);
                    filterString += kendo.format(format, fieldName, operator, value);
                }
            }

            if (idx < (length - 1)) {
                filterString += kendo.format(" {0} ", (filter.logic) ? filter.logic : filter.Logic);
            }
        }
        var addBracket = (filterString.split(' or ').length == 2 && (filterString.indexOf("(") === -1));
        return addBracket ? "("+ filterString + ")" : filterString;
    }

    //Config column template
    function getTemplate(item) {
        var template;
        var type = item.DataType.toLowerCase();
        if (type === "decimal") {
            template = '#= kendo.toString(' + item.Field + ', "n' + item.Precision + '") #';
        }

        if (item.IsDrilldown) {
            template = kendo.format("<a href=''>#: {0} #</a>", item.FieldAlias || item.Field);
        }

        if (type === "datetime") {
            template = kendo.format(datetimeTemplate, item.FieldAlias || item.Field);
        }

        if (item.IsDummy) {
            template = '<img src="../../../../../Content/Images/nav/drilldown.png" height="16" width="16"  style="float:middle">';
        }

        return template;
    }

    //Config inquiry grid
    function configGrid() {
        // Map array of primitive data to array of grid column object
        function getAggregations(items) {
            if (items == null || items.length == 0) {
                return {};
            }
            var aggreates = {};
            var fileds = InquiryGeneralViewModel.Aggregates;
            var fieldLength = fileds.length;
            for (i = 0; i < fieldLength; i++) {
                var func = {};
                var j = 5 * i;
                func.sum = items[j++];
                func.avg = items[j++];
                func.max = items[j++];
                func.min = items[j++];
                func.count = items[j++];
                aggreates[fileds[i]] = func;
            }
            return aggreates;
        }

        function getGridData(items) {
            var gridData = [], length = items.length;
            for (var i = 0; i < length; i++) {
                var row = items[i], len = row.length, r = {};
                for (var j = 0; j < len; j++) {
                    var presentation = cols[j].presentation;
                    var field = cols[j].field;
                    var type = cols[j].dataType.toLowerCase();
                    var value = row[j];

                    if (value && (type === "datetime" || type === "date" || type === "time")) {
                        value = (value.toString().length === 8) ? kendo.parseDate(value.toString(), 'yyyyMMdd') : value;
                    }
                    if (presentation) {
                        var list = presentation.filter(function (i) { return i.Value == value; });
                        if (list.length > 0) {
                            value = list[0].Text;
                        }
                    }
                    r[field] = value;
                }
                gridData.push(r);
            }
            return gridData;
        }

        function getGroupAggregations(items) {
            if (items == null) {
                return {};
            }
            var grpAggreates = {};
            var fields = InquiryGeneralViewModel.Fields;
            var idx = 1;
            var length = fields.length;
            for (var i = 0; i < length; i++) {
                var aggregates = fields[i].Aggregates;
                var len = aggregates.length;
                if (aggregates != null && len > 0) {
                    var func = {};
                    for (var j = 0; j < len; j++) {
                        var funcName = aggregates[j];
                        func[funcName] = items[idx++];
                    }
                    grpAggreates[fields[i].Field] = func;
                }
            }
            return grpAggreates;
        }

        function getGroups(groupItems, dataItems) {
            if (groupItems == null || groupItems.length == 0 || dataItems.length == 0) {
                return [];
            }
            var groupsData = [];
            var fieldName = InquiryGeneralViewModel.Groups[0].field;
            var dataType = grid.columns.filter(function (c) { return c.field == fieldName })[0].dataType;
            var isDateTime = dataType.toLowerCase() == "datetime";
            var fieldValues = dataItems.map(function (item) {
                var value = item[fieldName];
                return (isDateTime) ? kendo.toString(kendo.parseDate(value), "d") : value;
            });
            var groupValues = fieldValues.filter(function (v, i, self) { return self.indexOf(v) === i; });

            for (var i = 0, length = groupItems.length; i < length; i++) {
                var groupValue = groupValues[i];
                var groupsObj = { hasSubgroups: false, field: fieldName, value: groupValue };
                var aggregates = getGroupAggregations(groupItems[i]);
                groupsObj.aggregates = aggregates;
                groupsObj.items = dataItems.filter(function (item) {
                    var itemValue = item[fieldName];
                    if (isDateTime) {
                        itemValue = kendo.toString(kendo.parseDate(itemValue), "d");
                    }
                    return itemValue === groupValue;
                });
                groupsData.push(groupsObj);
            }
            return groupsData;
        }
        
        function convertFilterOperator(filter) {
            if (filter) {
                if (filter.filters && filter.filters.length > 0) {
                    filter.filters.forEach(function (f) {
                        convertFilterOperator(f);
                    });
                }
                if (filter.Operator) {
                    filter.operator = (filter.Operator == '=') ? 'eq' : filter.Operator ;
                    delete filter.Operator;
                }
            }
            return filter;
        }

        var dataSource = new kendo.data.DataSource({
            serverPaging: true,
            serverFiltering: true,
            serverSorting: (InquiryGeneralViewModel.Sql.length > 0), //true,
            serverAggregates: true,
            serverGrouping: true,
            aggregate: aggregates,
            pageSize: pageSize,
            filter: convertFilterOperator(InquiryGeneralViewModel.Filter),
            transport: {
                read: function (options) {
                    var filter = options.data.filter;
                    var filterString = getFilterString(filter);
                    var groupField = [];
                    InquiryGeneralViewModel.FilterString = filterString.length > 3 ? filterString : "";
                    InquiryGeneralViewModel.Sorts = options.data.sort;
                    InquiryGeneralViewModel.Aggregates = aggregateFields;
                    if (grid && grid.dataSource) {
                        InquiryGeneralViewModel.Groups = grid.dataSource.group();
                    }

                    var paramters = {
                        currentPageNumber: (grid) ? grid.dataSource.page() -1 : 0,
                        pageSize: pageSize,
                        viewModel: InquiryGeneralViewModel
                    };

                    var url = sg.utls.url.buildUrl("Core", "InquiryGeneral", "Get");
                    sg.utls.ajaxPost(url, paramters, function (successData) {
                        var gridData =  getGridData(successData.Items);
                        var aggegateData = getAggregations(successData.Aggregates);
                        var groupsData = getGroups(successData.Groups, gridData);
                        options.success({ data: gridData, aggregates: aggegateData, groups: groupsData, totalRecCount: successData.TotalResultsCount });
                        var checked = $("#chkShowGrandTotals").is(':checked');
                        checked ? $("#inquiryGrid .k-grid-footer").show() : $("#inquiryGrid .k-grid-footer").hide();
                    });
                }
            },
            schema: {
                aggregates: 'aggregates',
                total: 'totalRecCount',
                data: 'data',
                groups: 'groups'
            }
        });

        var cols = getGridColumns();
        var groupable = InquiryGeneralViewModel.IsAdhocQuery;
        if (InquiryGeneralViewModel.Groups && InquiryGeneralViewModel.Groups.length > 0) {
            dataSource.group(InquiryGeneralViewModel.Groups[0]);
        }
        
        grid = $("#inquiryGrid").kendoGrid({
            dataSource: dataSource,
            columns: cols,
            height: groupable ? 590 : 400,
            editable: false,
            navigatable: true,
            selectable: "row",
            scrollable: true,
            resizable: true,
            sortable: true,
            reorderable: !groupable,
            columnMenu: true,
            groupable: groupable,
            group: function(e) {
                if (e.groups.length == 0) {
                    return;
                }
                if (e.groups.length > 1) {
                    (e.groups[0].field == previousGroupName) ? e.groups.shift() : e.groups.pop();
                }
                previousGroupName = e.groups.field || e.groups[0].field;
            },

            filterable: {
                extra: true,
                operators: {
                    string: filterOperator
                }
            },
            columnMenuInit: function (e) {
                //Fix spanish localization issue for kendo grid filter
                if (kendo.culture().name.indexOf("es") > -1) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    var container = e.container;
                    var field = e.field;
                    var col = cols.filter(function (i) { return i.field === field });
                    if (col.length > 0 && col[0].dataType === "Decimal") {
                        menu.bind("open", function (e) {
                            var values = [];
                            UpdateDisplay();
                            var attrValue = "value: filters[0].operator";
                            var filterDropdown0 = container.find("[data-role=dropdownlist][data-bind='" + attrValue + "']").data("kendoDropDownList");
                            if (filterDropdown0) {
                                filterDropdown0.bind('change', UpdateDisplay);
                            }
                            attrValue = "value: filters[1].operator";
                            var filterDropdown1 = container.find("[data-role=dropdownlist][data-bind='" + attrValue + "']").data("kendoDropDownList");
                            if (filterDropdown1) {
                                filterDropdown1.bind('change', UpdateDisplay);
                            }

                            //Function to get filter values
                            function getFilterValues(filter, name) {
                                for (var idx = 0, length = filter.filters.length; idx < length; idx++) {
                                    var child = filter.filters[idx];
                                    if (child.filters && child.filters.length > 0) {
                                        getFilterValues(child, name);
                                    } else {
                                        if (child.field === name) {
                                            values.push(child.value);
                                        }
                                    }
                                }
                                return values;
                            }

                            //For spanish locale, update the display value, reset the numeric textbox with locale string
                            function UpdateDisplay() {
                                var filter = $("#inquiryGrid").data("kendoGrid").dataSource.filter();
                                if (filter) {
                                    values = []
                                    getFilterValues(filter, field);
                                    var numeric0 = container.find("[data-role=numerictextbox][data-bind*='0']").data("kendoNumericTextBox");
                                    var numeric1 = container.find("[data-role=numerictextbox][data-bind*='1']").data("kendoNumericTextBox");
                                    if (numeric0 && values.length > 0) {
                                        numeric0.value(values[0].toString().replace('.', ','));
                                    }
                                    if (numeric1 && values.length > 1) {
                                        numeric1.value(values[1].toString().replace('.', ','));
                                    }
                                }
                            }
                        });
                    }
                }
            },

            sortable: {
                allowUnsort: false
            },
            columnShow: function(e) {
            },
            columnHide: function (e) {
            },
            pageable: {
                input: true,
                numeric: false,
                refresh: true,
                change: function (e) {
                },
            },
            dataBound: function (e) {
                var columns = e.sender.columns;
                for (var i = 0, length = columns.length; i < length; i++) {
                    var col = columns[i];
                    if (col.isDrillDown) {
                        var condition = col.drillDownUrl && col.drillDownUrl.DrilldownCondition;
                        if (condition && condition.Field) {
                            var items = e.sender.dataSource.view();
                            for (var j = 0, len = items.length; j < len; j++) {
                                if (items[j][condition.Field] !== condition.Value) {
                                    //Remove the drill down link
                                    var row = e.sender.tbody.find("[data-uid='" + items[j].uid + "']");
                                    var cell = row.find("td:eq(" + i + ")");
                                    if (cell) {
                                        cell[0].innerHTML = kendo.htmlEncode(cell[0].innerText);
                                    }
                                }
                            }
                        }

                    }
                }
                var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "UnitsInStock" + "]").index();

                // iterate the data items and apply row styles where necessary
                var dataItems = e.sender.dataSource.view();
                for (var j = 0; j < dataItems.length; j++) {
                    var discontinued = dataItems[j].get("Discontinued");

                    var row = e.sender.tbody.find("[data-uid='" + dataItems[j].uid + "']");
                    if (discontinued) {
                        row.addClass("discontinued");
                    }
                }
            }
        }).data("kendoGrid");

        //Remove the column menu based on settings
        for (var i = 0; i < cols.length; i++) {
            var c = cols[i];
            if (!c.isFilterable) {
                var pattern = kendo.format("[data-field={0}]>.k-header-column-menu", c.field);
                grid.thead.find(pattern).remove();
            }
        }
        //Open popup window on drill down cell click
        $("#inquiryGrid").on("click", "tbody > tr > td > a", initShowPopup);
        $("#inquiryGrid").on("click", "tbody > tr > td > img", initShowPopup);
        //Init filter panel
        $("#filter").kendoGridFilterPanel({
            dataSource: dataSource,
            columns: grid.options.columns
        }).data("kendoGridFilterPanel");
    }

    //Init buttons
    function initButtons() {
        $("#btnPageSize10").click(function () {
            pageSize = 10;
            btnLink1.addClass('active');
            btnLink2.removeClass('active');
            btnLink3.removeClass('active');
            grid.dataSource.pageSize(pageSize);
        });

        $("#btnPageSize20").click(function () {
            pageSize = 50;
            btnLink2.addClass('active');
            btnLink1.removeClass('active');
            btnLink3.removeClass('active');
            grid.dataSource.pageSize(pageSize);
        });

        $("#btnPageSize30").click(function () {
            pageSize = 100;
            btnLink3.addClass('active');
            btnLink1.removeClass('active');
            btnLink2.removeClass('active');
            grid.dataSource.pageSize(pageSize);
        });

        function showHideFooter() {
            var checked = $("#chkShowGrandTotals").is(':checked');
            checked ? $("#inquiryGrid .k-grid-footer").show() : $("#inquiryGrid .k-grid-footer").hide();
        }

        $("#btnClearFilters").click(function () {
            grid.dataSource.filter({});
        });

        $("#chkShowGrandTotals").click(function () {
            $("#inquiryGrid .k-grid-footer").toggle(this.checked);
        });

        $("#btnSaveQuery").click(function () {
            window.parent.postMessage({ event_id: 'SaveQuery', templateId: InquiryGeneralViewModel.TemplateId }, '*');
        });

        $("#btnOpenQuery").click(function () {
            window.parent.postMessage({ event_id: 'OpenQuery' }, '*');
        });

        $("#btnExportQuery").click(function () {
            exportData();
        });

        $("#btnDeleteQuery").click(function () {
            var templateId = InquiryGeneralViewModel.TemplateId;
            var message = jQuery.validator.format(inquiryGeneralResources.DeleteMessage, InquiryGeneralViewModel.Title);
            sg.utls.showKendoConfirmationDialog(function () {
                var url = sg.utls.url.buildUrl("Core", "InquiryGeneral", "DeleteTemplate");
                sg.utls.ajaxPost(url, { "templateId": templateId }, deleteTemplate);
            }, null, message, "");
        });

        $("#btnDeleteQuery").prop("disabled", !InquiryGeneralViewModel.Deletable);
        $("#divShowGrandTotals > span").addClass("selected");
        $("#chkShowGrandTotals").prop("checked", true);

       function exportData() {
            var grid = $("#inquiryGrid").data("kendoGrid");
            var filter = grid.getOptions().dataSource.filter;
            var fields = grid.columns.filter(function (f) { return !f.hidden; });

            // Convert all dates in filter to a culture invariant format
            if (filter) {
               var datetimefields = fields.filter(function (f) { return f.dataType.toLowerCase() === "datetime"; }); // cache datetime fields for better performance
               filter = convertToCultureInvariantDate(filter, datetimefields);
            }

            var value = {
               templateId: InquiryGeneralViewModel.TemplateId,
               fields: fields,
               filter: filter
            };

            var url = sg.utls.url.buildUrl("Core", "InquiryGeneral", "Export");
            sg.utls.ajaxPost(url, value, exportDownload);
        }

        // prevent page refresh
        var form = document.getElementById('frmInquiryGeneral');
        form.onsubmit = function () {
            return false;
        };
    }

    //Init iFrame popup detail window
    function initShowPopup(e) {
        e.preventDefault();
        if (grid.dataSource.length <= 0) {
            return false;
        }
        var row = $(this).closest("tr");
        var colIndex = $(this).closest("td")[0].cellIndex;

        if (colIndex < 0) {
            return false;
        }
        if (InquiryGeneralViewModel.IsAdhocQuery) {
            var locked = $(this).closest(grid.lockedContent).length;
            colIndex = (colIndex > -1 && locked) ? colIndex : colIndex + 1;
        }

        var rowData = grid.dataItem(row);
        var col = grid.columns[colIndex];
        var colName = col.columnName;
        var param1 = rowData[colName];

        if (param1) {
            var childUrl = getDrillDownUrl(col, rowData);
            if (childUrl) {
                sg.utls.iFrameHelper.openWindow("InqueryDetailPopup", "", childUrl, null, null, null);
            }
        }
    }
 
    //Get drill down query url 
    function getDrillDownUrl(col, rowData) {
        var drillDownUrl = col.drillDownUrl;
        var param1 = rowData[col.columnName];
        var url = "";

        // Drill down url is in config json
        if (col.isDrillDown && drillDownUrl) {
            var controllerList = drillDownUrl.ControllerList;
            var controller;
            if (controllerList && controllerList.length == 1 ) {
                controller = controllerList[0].Controller;
            } else {
                var typeName = rowData[drillDownUrl.TypeField];
                var appName = (drillDownUrl.SrceApplField) ? rowData[drillDownUrl.SrceApplField] : "";
                if (appName) {
                    controller = controllerList.filter(function (c) { return c.Type == typeName && c.SrceAppl == appName; })[0];
                } else {
                    controller = controllerList.filter(function (c) { return c.Type == typeName; })[0];
                }
                // if the controller type is string name, get it's type id
                if (!controller) {
                    var typeCols = grid.columns.filter(function (c) { return c.field == drillDownUrl.TypeField; });
                    if (typeCols && typeCols.length > 0) {
                        var typeCol = typeCols[0];
                        if (typeCol.presentation) {
                            var types = typeCol.presentation.filter(function (p) { return p.Text == typeName; });
                            if (types && types.length > 0) {
                                var typeId = types[0].Value;
                                if (appName) {
                                    controller = controllerList.filter(function (c) { return c.Type == typeId && c.SrceAppl == appName; })[0];
                                } else {
                                    controller = controllerList.filter(function (c) { return c.Type == typeId; })[0];
                                }
                            }
                        }
                    }
                }

                if (!controller) {
                    controller = controllerList[0];
                }
                controller = controller.Controller;
            }
            //var params = drillDownUrl.Params;
            var params = controller.Parameters;
            var length = params.length;
            var paramsStr = "";
            if (controller.Controller === "InquiryGeneral") {
                paramsStr = "?templateId=" + params[0].Field;
                if (length > 1) {
                    var values = "";
                    for (var i = 1; i < length; i++) {
                        values += rowData[params[i].Field] + ",";
                    }
                    paramsStr += "&id=" + encodeURIComponent(values.slice(0, -1));
                }
            } else {
                var paramValues = InquiryGeneralViewModel.Ids;
                for (var i = 0; i < length; i++) {
                    var fieldName = params[i].Field;
                    var paramValue;
                    if (fieldName.indexOf('{') == 0 && fieldName.indexOf('}') > 1) {
                        var idx = fieldName.substring(1, 2);
                        paramValue = kendo.format(fieldName, (paramValues) ? paramValues[idx]: "");
                    } else {
                        paramValue = rowData[fieldName] || fieldName; 
                    }
                    paramsStr += kendo.format("{0}{1}={2}", (i == 0) ? "?" : "&", params[i].Name, encodeURIComponent(paramValue));
                }
            }
            url = sg.utls.url.buildUrl(controller.Area, controller.Controller, controller.Action) +"/" + paramsStr;
        }
        return url;
    }

    $(window).on('beforeunload', function () {
        sg.utls.releaseSession();
    });

    return {
        //init: initInquiryGeneral,
    };

}();

