// Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved.

"use strict";

var newTenantRepository = {
  
    save: function (data) {
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "NewTenant", "Save"), data, newTenantUISuccess.saveSuccess);
    }
};