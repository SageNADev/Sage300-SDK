// The MIT License (MIT) 
// Copyright (c) 1994-2023 The Sage Group plc or its licensors.  All rights reserved.
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

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    public class CodeSnippet
    {
        public const string JSReadOnlyGridDef = @"        
        //TODO: Get grid filter
        function get{Name}Filter(id) {
            return '';
        };

        // Define Grid Data Source
        let {Name}DataSource = new kendo.data.DataSource({
            serverPaging: true,
            serverFiltering: true,
            pageSize: 10,
            transport: {
                read: function (options) {
                    let finder = {};
                    finder.viewID = '{ViewId}';
                    finder.viewOrder = 0;
                    finder.displayFieldNames = [{Columns}];
                    finder.returnFieldNames = [{KeyColumn}];
                    //TODO: set finder filter
                    finder.filter = get{Name}Filter('');

                    var data = { finderOptions: finder };
                    data.finderOptions.PageNumber = options.data.page;
                    data.finderOptions.PageSize = 10;

                    sg.utls.ajaxPost(sg.utls.url.buildUrl('Core', 'ViewFinder', 'RefreshGrid'), data, function (successData) {
                        options.success({ data: successData.Data, totalRecCount: successData.TotalRecordCount });
                    });
                }
            },
            schema: {
                total: 'totalRecCount',
                data: 'data'
            }
        });

        //Init grid
        const {Name}Grid = $('#{Name}Grid').kendoGrid({
            autoBind: false,
            dataSource: {Name}DataSource,
            columns: get{Name}GridColumns(),
            editable: false,
            navigatable: true,
            selectable: true,
            scrollable: true,
            resizable: true,
            pageable: {
                input: true,
                numeric: false,
                refresh: false
            },
        }).data('kendoGrid');

        //Initialize and show drill down popup.
        function initShowPopup(e) {
            e.preventDefault();
            const gridId = e.delegateTarget.id;
            const grid = $('#' + gridId).data('kendoGrid');
            if (grid.dataSource.data().length <= 0) {
                return false;
            }
            const colIndex = $(event.target).closest('td')[0].cellIndex;
            if (colIndex < 0) {
                return false;
            }
            const row = $(event.target).closest('tr'),
                rowData = grid.dataItem(row),
                col = grid.columns[colIndex];

            if (col.drillDownUrl) {
                const urls = col.drillDownUrl.split('/');
                if (urls.length > 2) {
                    var url = sg.utls.url.buildUrl(urls[0], urls[1], urls[2]) + '?id='+ rowData[col.field];
                    sg.utls.iFrameHelper.openWindow('GridKeyColumnDetailPopup', '', url);
                }
            }
        }

        //Binding the drilldown popup window
        $('#{Name}Grid').on('click', 'tbody > tr > td > a', initShowPopup);
        $('#{Name}Grid').on('click', 'tbody > tr > td > img', initShowPopup);

        //Support grid keyboard up/down pageup/down selection
        {Name}Grid.table.on('keydown', (e) => {
            if ({Name}Grid.select().index() === 0 && e.code === 'ArrowUp') {
                let cell = customerGrid.tbody.find('tr:first td:first');
                {Name}Grid.current(cell);
                {Name}Grid.table.focus();
            }
            if ([38, 40].indexOf(e.keyCode) >= 0) {
                setTimeout(() => {
                    {Name}Grid.select($('#{Name}Grid_active_cell').closest('tr'));
                });
            }
        })

        // Get column template
        function getColumnTemplate(data, field, dataType, url) {
            let value = data[field] || '';
            let template = value;

            switch (dataType) {
                case 'String':
                    const text = sg.utls.htmlEncode(value);
                    template = url ? kendo.format(""<a href=''>{0}</a>"", text) : text;
                    break;

                case 'Decimal':
                case 'Double':
                case 'Integer':
                case 'Long':
					let precision = dataType === 'Integer' || dataType === 'Long'? 0: 3;
                    template = '<span style=""float:right"">' + sg.utls.kndoUI.getFormattedDecimalNumber(value || 0, precision) + '</span>';
                    break;
           
                case 'DateTime':
                    template = kendo.toString(kendo.parseDate(value), ""d"");
                    break;
            }
            return template;
        };

        //Get grid columns
        function get{Name}GridColumns()
        {
            let cols = [];
            const titles = [{Titles}]; 
            const fileds = [{Columns}];
            const types = [{Types}];
            const urls = [{Urls}];
            
            for (let i = 0; i < titles.length; i++) {
                var col = {};
                col.title = titles[i];
                col.field = fileds[i];
                col.template = (data) => getColumnTemplate(data, fileds[i], types[i], urls[i]);
                col.drillDownUrl = urls[i];
                cols.push(col);
            }
            return cols;
        };";

        public const string HASOptionalFieldDef = @"
        /// <summary>
        ///  Gets or sets HasOptionalFields
        /// </summary>
        public bool HasOptionalFields { get; set; }
        ";

        public const string JSOptionalFieldHamburgerDef = @"
        var listOptionalField = [sg.utls.labelMenuParams('lnkOpenOptionalFields', 'Add/Edit', {MODELNAME}UI.showOptionalField, null)];
        LabelMenuHelper.initialize(listOptionalField, 'lnkHasOptionalFields', '{MODELNAME}UI.{MODELNAME}Model'); 
";

        public const string JSOptionalFieldPopUpDef = @"
        sg.utls.intializeKendoWindowPopup('#optionalField', 'Optional Fields', function() {
            var count = $('#{OFGRIDNAME}').data('kendoGrid').dataSource.total();
            modelData.NumberOfOptionalFields(count);
        });
        sg.utls.intializeKendoWindowPopup('#detailOptionalField', 'Detail Optional Fields', function() {
            sg.optionalFieldControl.closePopUp('{OFDGRIDNAME}', '{DGRIDNAME}');
        });
";

        public const string JSShowOptionalFieldDef = @"
        //Show detail optional field in popup window
        showDetailOptionalField: function () {
            var grid = $('#{DGRIDNAME}').data('kendoGrid'),
                selectedRow = sg.utls.kndoUI.getSelectedRowData(grid),
                filter = kendo.format('{SEQ}={0} AND {LINENO}={1}', selectedRow.{SEQ}, selectedRow.{LINENO}),
                isReadOnly = false;
            sg.optionalFieldControl.showPopUp('{OFDGRIDNAME}', 'detailOptionalField', isReadOnly, filter, '{DGRIDNAME}');
        },

        //Show optional field in popup window
        showOptionalField: function () {
            sg.optionalFieldControl.showPopUp('{OFGRIDNAME}', 'optionalField', false);
        },

";

        public const string JSSyncHeaderDetailGrid = @"
    //Sync header detail grids, the following commented code snippet is Sync header/detail grid
    syncHeaderDetailGrid: function (record) {
        //let row = record || sg.viewList.currentRecord('{HeaderGrid}');
        //if (row.{DetailGridKey}) {
        //    sg.viewList.filter('{DetailGrid}', '{DetailGridKey} = ' + row.{DetailGridKey});
        //}
        //$('#{DetailGrid}').data('kendoGrid').dataSource.read();
    },

    //Grid custom plug in function, defined in Grid definition Json file
    customGridAfterLoadData: function(data)
    {
        {EntityName}UISuccess.syncHeaderDetailGrid(data[0]);
    },

    //Grid custom plug in function, defined in Grid definition Json file
    customGridAfterSetActiveRecord: function(record)
    {
        {EntityName}UISuccess.syncHeaderDetailGrid(record);
    },

";

        public const string JSCustomFunctionsDef = @"
    //Grid column custom plug in function, defined in Grid column definition Json file
    showPencilIcon: function (e, column) {
        //TODO: based on requirement, add custom code here. The following commented code snippet is change finder icon to pencil icon
        //let template = ""<div class='pencil-wrapper' onclick='{EntityName}UISuccess.pencilHandler()'><span class='pencil-txt'>#:{0}#</span><span class='pencil-icon'><input class='icon pencil-edit' type='button'></span></div>"";
        //template = kendo.format(template, column.FieldName);
        //column.Template = kendo.template(template);
    },

    //Custom plug in code
    pencilHandler: function () {
        alert('Add custom code here.'); 
    },

    //Grid column custom plug in function, defined in Grid column definition Json file
    changeFinderDefinition: function(e, finder)
    {
        //TODO: based on requirement, add custom code here. The following commented code snippet is dynamically change finder
        //finder.viewID = 'AP0006';
        //finder.returnFieldNames = ['ACCTSET'];
        //finder.displayFieldNames = ['ACCTSET', 'TEXTDESC', 'SWACTV', 'DATEINACTV', 'DATELASTMN', 'CURRCODE'];
    },

    //Grid column custom plug in function, defined in Grid column definition Json file
    showHideFinderButton: function(e, finder) {
        //TODO: based on requirement, add custom code here. The following commented code snippet is hide finder
        //let id = finder.buttonId;
        //if (id) {
        //    $('#' + id).hide();
        //}
    },

";
    }
}
