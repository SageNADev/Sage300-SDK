/// <reference path="../TestUtil/GlobalTestUtils.js" />
/// <reference path="../../Areas/Shared/Scripts/Sage.CA.SBS.ERP.Sage300.Common.KendoHelpers.js" />

/* Target scripts*/
/// <reference path="../../Areas/Shared/Scripts/Sage.CA.SBS.ERP.Sage300.Common.Customization.js" />

describe("Sage.CA.SBS.ERP.Sage300.Common.Customization.js tests", function () {
   var testHTMLDiv;

    beforeEach(function() {
        testHTMLDiv = '<div class="top-buttons right">' +
            '<input class="btn-primary" data-sage300uicontrol="type:Button,name:CreateNew,changed:0" id="btnCreateNew" name="CreateNew" type="button" value="Create New"></input>' +
            '</div>';

        $(testHTMLDiv).appendTo("body");
    });

    it("Test updateControl", function() {
        var control = new Object();
        
        control.Name = "CreateNew";
        control.Type = "Button";
        control.Text = "XXX";

        // update the button caption
        Customization.updateControl(control, false);

        expect($('#btnCreateNew').text()).toBe("XXX");
    });

    it("Test guid", function () {
        expect(Customization.guid().search(/[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{8}/i)).toBe(0);
    });
})