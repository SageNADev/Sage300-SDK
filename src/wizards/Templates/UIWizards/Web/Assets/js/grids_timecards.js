var Data = [
	{
     lineumber: '',
     details: '',
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
$("#GridTime").kendoGrid({
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
          template: "<input type='button' class='icon pencil-edit' name='btnDetails' id='btnDetails'>",
          attributes: { 'class': 'align-center' },
          field: "details",
          title: "Taxes",
          width: 60
      },
      {
          field: "field1",
          title: "Contract",
          width: 120
      },
      {
          field: "field2",
          title: "Project",
          width: 120
      },
      {
          field: "field3",
          title: "Category",
          width: 120
      },
      {
          field: "field4",
          title: "Saturday 4",
          width: 100
      },
      {
          field: "field5",
          title: "Sunday 5",
          width: 100
      },
      {
          field: "field6",
          title: "Monday 6",
          width: 100
      },
      {
          field: "field7",
          title: "Tuesday 7",
          width: 100
      },
      {
          field: "field8",
          title: "Wednesday 8",
          width: 100
      },
      {
          field: "field9",
          title: "Thursday 9",
          width: 100
      },
      {
          field: "field10",
          title: "Friday 10",
          width: 100
      },
      {
          field: "field11",
          title: "Total",
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
$("#GridDetails").kendoGrid({
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

var Data = [
  {
     lineumber: '',
     details: '',
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
     field14: '',
     field15: '',
     field16: '',
     field17: '',
     field18: '',
     field19: ''
  }
];
$("#GridExpense").kendoGrid({
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
          template: "<input type='button' class='icon pencil-edit' name='btnDetails' id='btnDetails'>",
          attributes: { 'class': 'align-center' },
          field: "details",
          title: "Taxes",
          width: 60
      },
      {
          field: "field1",
          title: "Contract",
          width: 120
      },
      {
          field: "field2",
          title: "Project",
          width: 120
      },
      {
          field: "field3",
          title: "Category",
          width: 120
      },
      {
          field: "field4",
          title: "Cost Class",
          width: 120
      },
      {
          field: "field5",
          title: "Resource",
          width: 120
      },
      {
          field: "field6",
          title: "Expense Code",
          width: 120
      },
      {
          field: "field7",
          title: "Expense Code Description",
          width: 120
      },
      {
          field: "field8",
          title: "Transaction Date",
          width: 120
      },
      {
          field: "field9",
          title: "Employee Expense Account",
          width: 120
      },
      {
          field: "field10",
          title: "Employee Expense Account Description",
          width: 120
      },
      {
          field: "field11",
          title: "Work in Progress/Cost of Sales Account",
          width: 120
      },
      {
          field: "field12",
          title: "Work in Progress/Cost of Sales Account Description",
          width: 120
      },
      {
          field: "field13",
          title: "Cost Amount",
          width: 120
      },
      {
          field: "field14",
          title: "Billing Type",
          width: 120
      },
      {
          field: "field15",
          title: "Billing Amount",
          width: 120
      },
      {
          field: "field16",
          title: "A/R Item Number",
          width: 120
      },
      {
          field: "field17",
          title: "Unit of Measure",
          width: 120
      },
      {
          field: "field18",
          title: "Comments",
          width: 120
      },
      {
          field: "field19",
          title: "Optional Fields",
          width: 120
      }
  ],
  editable: false
});

var Data = [
  {
     lineumber: '',
     details: '',
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
     field14: '',
     field15: '',
     field16: '',
     field17: '',
     field18: '',
     field19: '',
     field20: '',
     field21: ''
  }
];
$("#GridTimeDetail").kendoGrid({
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
          template: "<input type='button' class='icon pencil-edit' name='btnDetails' id='btnDetails'>",
          attributes: { 'class': 'align-center' },
          field: "details",
          title: "Taxes",
          width: 60
      },
      {
          field: "field1",
          title: "Contract",
          width: 120
      },
      {
          field: "field2",
          title: "Project",
          width: 120
      },
      {
          field: "field3",
          title: "Category",
          width: 120
      },
      {
          field: "field4",
          title: "Billing Type",
          width: 120
      },
      {
          field: "field5",
          title: "Earning Code",
          width: 120
      },
      {
          field: "field6",
          title: "Transaction Date",
          width: 120
      },
      {
          field: "field7",
          title: "Start Time",
          width: 120
      },
      {
          field: "field8",
          title: "End Time",
          width: 120
      },
      {
          field: "field9",
          title: "Hours",
          width: 120
      },
      {
          field: "field10",
          title: "Unit Cost",
          width: 120
      },
      {
          field: "field11",
          title: "Extended Cost",
          width: 120
      },
      {
          field: "field12",
          title: "Billing Rate",
          width: 120
      },
      {
          field: "field13",
          title: "Extended Billing Amount",
          width: 120
      },
      {
          field: "field14",
          title: "Payroll Account",
          width: 120
      },
      {
          field: "field15",
          title: "Payroll Account Description",
          width: 120
      },
      {
          field: "field16",
          title: "Work in Progress/Cost of Sales Account",
          width: 120
      },
      {
          field: "field17",
          title: "Work in Progress/Cost of Sales Account Description",
          width: 120
      },
      {
          field: "field18",
          title: "A/R Item Number",
          width: 120
      },
      {
          field: "field19",
          title: "Unit of Measure",
          width: 120
      },
      {
          field: "field20",
          title: "Comments",
          width: 120
      },
      {
          field: "field20",
          title: "Optional Fields",
          width: 120
      }
  ],
  editable: false
});