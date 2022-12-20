(function() {
    'use strict';
    /** Kendo custom editor object */
    let kendoCustomEditorObject = {

        //override customTemplate here 
        /*customTemplate: function (container, options) {
            //add code here
        },*/
    }

    this.kendoStaticCustomEditor = baseKendoStaticCustomEditor.extend({}, kendoCustomEditorObject);

}).call(this); 