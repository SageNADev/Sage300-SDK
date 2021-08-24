var Data = [
	{
     lineumber: '',
     field1: '',
	   field2: '',
     field3: '',
     field4: '',
     field5: '',
     field6: '',
     field7: '',
     field8: '',
     field9: '',
     field10: '',
     field11: ''
	}
];
$("#GridEarningsDeductions").kendoGrid({
    dataSource: {
      data: Data,
      schema: {
          model: {
              fields: {
                  field1: { editable: false },
                  value: { type: "number" },
                  description: { type: "string" },
                  amount: { type: "number" },
              }
          }
      },
      pageSize: 10
  },
  scrollable: true,
  sortable: true,
  selectable: true,
  resizable: true,
  pageable: {
      input: true,
      numeric: false,
      refresh: false
  },
  columns: [
      {
          field: "linenumber",
          title: "Line Number",
          width: 100
      },
      {
          field: "field1",
          title: "Earning/Deduction Code",
          width: 200
      },
      {
          field: "field2",
          title: "Earning/Deduction Description",
          width: 220
      },
      {
          field: "field3",
          title: "Type",
          width: 120
      },
      {
          field: "field4",
          title: "Date",
          width: 100
      },
      {
          field: "field5",
          title: "Start Time",
          width: 100
      },
      {
          field: "field6",
          title: "Stop Time",
          width: 100
      },
      {
          field: "field7",
          title: "Hours",
          width: 100
      },
      {
          field: "field8",
          title: "Pieces/",
          width: 100
      },
      {
          field: "field9",
          title: "Lorem Ipsum",
          width: 100
      },
      {
          field: "field10",
          title: "Lorem Ipsum",
          width: 100
      },
      {
          field: "field11",
          title: "Lorem Ipsum",
          width: 100
      }
  ],
  editable: false
});

var Data1 = [
  {
     field1: 'STATE',
     field2: 'State Tax',
     field3: '1',
     field4: 'Taxable item',
     field5: 'No'
  },
  {
     field1: 'COUNTRY',
     field2: 'Country Tax',
     field3: '1',
     field4: 'Taxable merchandise',
     field5: 'No'
  }
];
$("#GridTaxDetails").kendoGrid({
    dataSource: {
      data: Data1,
      schema: {
          model: {
              fields: {
                  field1: { editable: false },
                  value: { type: "number" },
                  description: { type: "string" },
                  amount: { type: "number" },
              }
          }
      },
      pageSize: 4
  },
  scrollable: true,
  sortable: true,
  selectable: true,
  resizable: true,
  // pageable: {
  //     input: true,
  //     numeric: false,
  //     refresh: false
  // },
  columns: [
      {
          field: "field1",
          title: "Tax Authority",
          width: 120
      },
      {
          field: "field2",
          title: "Tax Authority Description",
          width: 200
      },
      {
          field: "field3",
          title: "Tax Class",
          attributes: { 'class': 'numeric' },
          width: 100
      },
      {
          field: "field4",
          title: "Tax Class Description",
          width: 200
      },
      {
          field: "field5",
          title: "Tax Included",
          width: 100
      }
  ],
  editable: false
});

var Data2 = [
  {
     // lineumber: '',
     field1: '',
     field2: '',
     field3: '',
     field4: '',
     field5: '',
     field6: '',
     field7: '',
     field8: '',
     field9: '',
     field10: '',
     field11: '',
     field12: '',
     field13: '',
     field14: ''
  }
];
$("#GridEarningsExpenses").kendoGrid({
    dataSource: {
      data: Data2,
      schema: {
          model: {
              fields: {
                  field1: { editable: false },
                  value: { type: "number" },
                  description: { type: "string" },
                  amount: { type: "number" },
              }
          }
      },
      pageSize: 10
  },
  scrollable: true,
  sortable: true,
  selectable: true,
  resizable: true,
  pageable: {
      input: true,
      numeric: false,
      refresh: false
  },
  columns: [
      // {
      //     field: "linenumber",
      //     title: "Line Number",
      //     width: 100
      // },
      {
          field: "field1",
          title: "Date",
          width: 100
      },
      {
          field: "field2",
          title: "Day of Week",
          width: 120
      },
      {
          field: "field3",
          title: "Earning/Expense/Accrual",
          width: 160
      },
      {
          field: "field4",
          title: "Start Time",
          width: 100
      },
      {
          field: "field5",
          title: "Stop Time",
          width: 100
      },
      {
          field: "field6",
          title: "Hours",
          width: 100
      },
      {
          field: "field7",
          title: "Tips/Expenses/Pieces/Sales",
          width: 160
      },
      {
          field: "field8",
          title: "Tips Based On",
          width: 100
      },
      {
          field: "field9",
          title: "Pooled Tips",
          width: 100
      },
      {
          field: "field10",
          title: "Shift Schedule",
          width: 100
      },
      {
          field: "field11",
          title: "Shift Number",
          width: 100
      },
      {
          field: "field12",
          title: "Use Optional Fields",
          width: 100
      },
      {
          field: "field13",
          title: "Work Classification",
          width: 100
      },
      {
          field: "field14",
          title: "Jobs",
          width: 100
      }
  ],
  editable: false
});