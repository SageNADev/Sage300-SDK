/* Copyright (c) 1994-2016 Sage Software, Inc. All rights reserved */

//Prepare some sample data for unit tests
var TestUtils = {};
TestUtils.constants = {
    PostedStatus: 3,
    OpenStatus: 2,
    PostUrl: "postUrl",
    ErrorUrl: "errorUrl",
    Message1: "message1",
    Message2: "message2",
    Message3: "message3"
};

TestUtils.rowHelper = {
    unpostedRow: {
        BatchStatus: TestUtils.constants.OpenStatus,
        PostingSequence: 0,
        NoofErrors: 0
    },
    postedRow: {
        BatchStatus: TestUtils.constants.PostedStatus,
        PostingSequence: 1,
        NoofErrors: 0
    },
    postedErrorRow: {
        BatchStatus: 3,
        PostingSequence: 2,
        NoofErrors: 1
    }
};