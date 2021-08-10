var Data = [
	{
	   optionalField: '',
	   optionalFieldDescription: '',
	   valueSet: '',
	   defaultValue: '',
     valueDescription: '',
	   required: "",
     auto: ""
	},
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  },
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  },
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  },
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  },
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  },
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  },
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  },
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  },
  {
     optionalField: '',
     optionalFieldDescription: '',
     valueSet: '',
     defaultValue: '',
     valueDescription: '',
     required: "",
     auto: ""
  }
];
$("#GridOptionalFields").kendoGrid({
    dataSource: {
      data: Data,
      schema: {
          model: {
              fields: {
                  checkbox: { editable: false, type: "string" },
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
  // height: 152,
  selectable: true,
  resizable: true,
  pageable: {
      input: true,
      numeric: false,
      refresh: false
  },
  columns: [
      {
          field: "optionalField",
          title: "Optional Field",
          width: 150
      },
      {
          field: "optionalFieldDescription",
          title: "Optional Field Description",
          width: 200
      },
      {
          field: "valueSet",
          title: "Value Set",
          width: 100
      },
      {
          field: "defaultValue",
          title: " Value",
          width: 150
      },
      {
          field: "valueDescription",
          title: "Value Description",
          width: 200
      }
  ],
  editable: false
});

