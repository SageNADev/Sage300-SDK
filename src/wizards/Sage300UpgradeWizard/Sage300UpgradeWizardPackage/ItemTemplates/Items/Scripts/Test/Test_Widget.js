/// <reference path="../Portal/Widget.js" />

NavigableMenuDetail = { WidgetOrders: "{ \"A\": \"1\" }" };
homeViewModel = null;

describe("Widget.js tests", function () {

    beforeEach(function () {
        // something to init for each test
    });

    afterEach(function () {
        // something to clean up after test
    });

    it("Test slotManager.sortSlotsByRank", function () {
        slotManager.slots = [{ slotId: 2, widgetId: 2, rank: 2 },
                             { slotId: 1, widgetId: 1, rank: 1 },
                             { slotId: 3, widgetId: 3, rank: 3 },
                             { slotId: 5, widgetId: 5, rank: 5 },
                             { slotId: 4, widgetId: 4, rank: 4 }, ];

        slotManager.sortSlotsByRank();

        expect(slotManager.slots[0].rank).toBe(1);
        expect(slotManager.slots[1].rank).toBe(2);
        expect(slotManager.slots[2].rank).toBe(3);
        expect(slotManager.slots[3].rank).toBe(4);
        expect(slotManager.slots[4].rank).toBe(5);
    });

    it("Test slotManager.recalculateRanks", function () {
        slotManager.slots = [{ slotId: 2, widgetId: 2, rank: 12 },
                             { slotId: 1, widgetId: 1, rank: 11 },
                             { slotId: 3, widgetId: 3, rank: 13 },
                             { slotId: 5, widgetId: 5, rank: 15 },
                             { slotId: 4, widgetId: 4, rank: 14 }, ];

        slotManager.recalculateRanks();

        expect(slotManager.slots[0].rank).toBe(1);
        expect(slotManager.slots[1].rank).toBe(2);
        expect(slotManager.slots[2].rank).toBe(3);
        expect(slotManager.slots[3].rank).toBe(4);
        expect(slotManager.slots[4].rank).toBe(5);
    });

    it("Test slotManager.getSlotById", function () {
        slotManager.slots = [{ slotId: 2, widgetId: 2, rank: 2 },
                             { slotId: 1, widgetId: 1, rank: 1 },
                             { slotId: 3, widgetId: 3, rank: 3 },
                             { slotId: 5, widgetId: 5, rank: 5 },
                             { slotId: 4, widgetId: 4, rank: 4 }, ];

        expect(slotManager.getSlotById(3).slotId).toBe(3);
    });

    it("Test slotManager.registerSlot", function () {
        slotManager.slots = [{ slotId: 2, widgetId: 2, rank: 2 },
                             { slotId: 1, widgetId: 1, rank: 1 },
                             { slotId: 3, widgetId: 3, rank: 3 },
                             { slotId: 5, widgetId: 5, rank: 5 },
                             { slotId: 4, widgetId: 4, rank: 4 }, ];

        var newSlot = slotManager.registerSlot(6, 6, 6);

        expect(newSlot.slotId).toBe(6);
        expect(slotManager.slots[slotManager.slots.length - 1].widgetId).toBe(6);
    });

    it("Test slotManager.unregisterSlot", function () {
        slotManager.slots = [{ slotId: 2, widgetId: 2, rank: 2 },
                             { slotId: 1, widgetId: 1, rank: 1 },
                             { slotId: 3, widgetId: 3, rank: 3 },
                             { slotId: 5, widgetId: 5, rank: 5 },
                             { slotId: 4, widgetId: 4, rank: 4 }, ];

        expect(slotManager.unregisterSlot(4)).toBeTruthy();
        expect(slotManager.getSlotById(4)).toBeNull();
    });
});