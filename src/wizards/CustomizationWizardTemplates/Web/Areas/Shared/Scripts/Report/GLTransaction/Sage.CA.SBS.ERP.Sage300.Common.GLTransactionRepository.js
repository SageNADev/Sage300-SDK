// Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved.
"use strict";

var GLTransactionRepository = GLTransactionRepository || {};
var GLTransactionRepository = {

    executeGLTransaction: function (data) {

        var module;
        
        if (data.SourceApplication == GLTransactionUI.Module.Bank) {
            module = GLTransactionUI.Module.CommonServices;
        } else {
            module = data.SourceApplication;
        }
        sg.utls.ajaxPost(sg.utls.url.buildUrl(module, "GLTransaction", "Execute"), data, onSuccess.executeGLTransaction);
    },

}