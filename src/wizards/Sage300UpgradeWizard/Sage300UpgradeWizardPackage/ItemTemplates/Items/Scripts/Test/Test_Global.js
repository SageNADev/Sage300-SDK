/// <reference path="../../Areas/Shared/Scripts/Sage.CA.SBS.ERP.Sage300.Common.Cache.js" />
/// <reference path="../TestUtil/GlobalTestUtils.js" />
/// <reference path="../../Areas/Shared/Scripts/Sage.CA.SBS.ERP.Sage300.Common.Global.js" />

describe("Global.js tests", function () {

    beforeEach(function () {
        // something to init for each test
    });

    afterEach(function () {
        // something to clean up after test
    });

    it("Test Global.utls.regExp", function () {
        expect(sg.utls.regExp.TIME).toBe("([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]");
        expect(sg.utls.regExp.COMMA).toBe("\,");
        expect(sg.utls.regExp.ONLYGLOBAL).toBe("g");
    });

    it("Test Global.sg.utls.getFirefoxSpecialKeys", function () {
        var result = sg.utls.getFirefoxSpecialKeys({ charCode: 0 });
        var expectResult = [8, 9, 46, 37, 39, 35, 36];
        expect($(result).not(expectResult).length == 0 && $(expectResult).not(result).length == 0).toBeTruthy();

        // mock the isCtrlKeyPressed to force to return true!!!
        spyOn(sg.utls, 'isCtrlKeyPressed').and.returnValue(true);
        result = sg.utls.getFirefoxSpecialKeys({ charCode: 1 });
        expectResult = [8, 9, 99, 118];
        expect($(result).not(expectResult).length == 0 && $(expectResult).not(result).length == 0).toBeTruthy();
    });
    
    it("Test Global.utls.msgType", function () {
        expect(sg.utls.msgType.ERROR).toBe("error");
        expect(sg.utls.msgType.INFO).toBe("info");
        expect(sg.utls.msgType.SUCCESS).toBe("success");
        expect(sg.utls.msgType.WARNING).toBe("warning");
    });

    it("Test Global.utls.toFixedDown", function () {
        //positive
        expect(sg.utls.toFixedDown(10.123, 2)).toBe(10.12);
        expect(sg.utls.toFixedDown(10.12, 2)).toBe(10.12);
        expect(sg.utls.toFixedDown(10.1, 2)).toBe(10.1);
        expect(sg.utls.toFixedDown(10, 2)).toBe(10);
        //negative
        expect(sg.utls.toFixedDown(-10.123, 2)).toBe(-10.12);
        expect(sg.utls.toFixedDown(-10.12, 2)).toBe(-10.12);
        expect(sg.utls.toFixedDown(-10.1, 2)).toBe(-10.1);
        expect(sg.utls.toFixedDown(-10, 2)).toBe(-10);
        //no round up
        expect(sg.utls.toFixedDown(10.899, 2)).toBe(10.89);
    });

    it("Test Global.utls.formatFiscalPeriod", function () {
        expect(sg.utls.formatFiscalPeriod("14")).toBe("ADJ");
        expect(sg.utls.formatFiscalPeriod("15")).toBe("CLS");
        expect(sg.utls.formatFiscalPeriod("014")).not.toBe("ADJ");
        expect(sg.utls.formatFiscalPeriod("014")).toBe("014");
        expect(sg.utls.formatFiscalPeriod("015")).not.toBe("CLS");
        expect(sg.utls.formatFiscalPeriod("Anything else")).toBe("Anything else");
        expect(sg.utls.formatFiscalPeriod(100)).toBe(100);
    });
});