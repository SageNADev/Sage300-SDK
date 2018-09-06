/// <reference path="../TestUtil/BatchGridHelperTestUtils.js" />
/// <reference path="../../Areas/Core/Scripts/Sage.CA.SBS.ERP.Sage300.Common.BatchGridHelper.js" />

describe("BatchGridHelper.js tests", function () {
    var testSubject;

    beforeEach(function () {
        // something to init for each test
        testSubject = new BatchGridHelper.appendJournalLink(null, TestUtils.constants.PostUrl, TestUtils.constants.ErrorUrl, TestUtils.constants.Message1, TestUtils.constants.Message2, TestUtils.constants.Message3, TestUtils.constants.PostedStatus);
        spyOn(sg.utls.iFrameHelper, "openWindow");
        spyOn(sg.utls, "showMessageInfo");
    });

    afterEach(function () {
        // something to clean up after test
        testSubject = null;
    });

    it("Test BatchGridHelper PostJournal with unposted row behaviour", function () {
        spyOn(testSubject, "getSelectedRowData").and.returnValue(TestUtils.rowHelper.unpostedRow);

        testSubject.btnPostingJournalClickHandler();

        expect(testSubject.getSelectedRowData.calls.count()).toBe(1);
        expect(sg.utls.showMessageInfo).toHaveBeenCalledWith(sg.utls.msgType.ERROR, TestUtils.constants.Message1);
        expect(sg.utls.iFrameHelper.openWindow).not.toHaveBeenCalled();
    });

    it("Test BatchGridHelper PostJournal with posted row behaviour", function () {
        spyOn(testSubject, "getSelectedRowData").and.returnValue(TestUtils.rowHelper.postedRow);

        testSubject.btnPostingJournalClickHandler();

        expect(testSubject.getSelectedRowData.calls.count()).toBe(1);
        expect(sg.utls.showMessageInfo).not.toHaveBeenCalled();
        expect(sg.utls.iFrameHelper.openWindow).toHaveBeenCalledWith(jasmine.any(String), "", TestUtils.constants.PostUrl + TestUtils.rowHelper.postedRow.PostingSequence, jasmine.any(Number), jasmine.any(Number));
    });

    it("Test BatchGridHelper PostError with unposted row behaviour", function () {
        spyOn(testSubject, "getSelectedRowData").and.returnValue(TestUtils.rowHelper.unpostedRow);

        testSubject.btnPostingErrorClickHandler();

        expect(testSubject.getSelectedRowData.calls.count()).toBe(1);
        expect(sg.utls.showMessageInfo).toHaveBeenCalledWith(sg.utls.msgType.ERROR, TestUtils.constants.Message2);
        expect(sg.utls.iFrameHelper.openWindow).not.toHaveBeenCalled();
    });

    it("Test BatchGridHelper PostError with posted row behaviour", function () {
        spyOn(testSubject, "getSelectedRowData").and.returnValue(TestUtils.rowHelper.postedRow);

        testSubject.btnPostingErrorClickHandler();

        expect(testSubject.getSelectedRowData.calls.count()).toBe(1);
        expect(sg.utls.showMessageInfo).toHaveBeenCalledWith(sg.utls.msgType.ERROR, TestUtils.constants.Message3);
        expect(sg.utls.iFrameHelper.openWindow).not.toHaveBeenCalled();
    });

    it("Test BatchGridHelper PostError with posted error row behaviour", function () {
        spyOn(testSubject, "getSelectedRowData").and.returnValue(TestUtils.rowHelper.postedErrorRow);

        testSubject.btnPostingErrorClickHandler();

        expect(testSubject.getSelectedRowData.calls.count()).toBe(1);
        expect(sg.utls.showMessageInfo).not.toHaveBeenCalled();
        expect(sg.utls.iFrameHelper.openWindow).toHaveBeenCalledWith(jasmine.any(String), "", TestUtils.constants.ErrorUrl + TestUtils.rowHelper.postedErrorRow.PostingSequence, jasmine.any(Number), jasmine.any(Number));
    });

    it("Test BatchGridHelper.journalPostingColumnTemplate", function () {
        var result = BatchGridHelper.journalPostingColumnTemplate(3)
        expect(result).toBe('<div class="pencil-wrapper"><span class="pencil-txt">3</span><span class="pencil-icon"><input type="button" class="icon edit-field btnPostingSequence"/></span></div>');
    });

    it("Test BatchGridHelper.journalErrorColumnTemplate", function () {
        var result = BatchGridHelper.journalErrorColumnTemplate(3)
        expect(result).toBe('<div class="pencil-wrapper"><span class="pencil-txt">3</span><span class="pencil-icon"><input type="button" class="icon edit-field btnPostingErrors"/></span></div>');
    });
});