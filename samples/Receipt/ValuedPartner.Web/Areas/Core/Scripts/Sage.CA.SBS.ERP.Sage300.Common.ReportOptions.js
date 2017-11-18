/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

(function (sg, $) {
    sg.reportOptions = {
        setSaveDefaults: function (url, getData, onSuccess) {
            $("#btnSaveUserPreference").on('click', function () {
                var data = getData.call();
                sg.utls.ajaxPost(url, data, onSuccess);
            });
        },
        setClearDefaults: function (url, onSuccess) {
            var data = {};
            $("#btnClearUserPreference").on('click', function () {
                sg.utls.ajaxPost(url, data, onSuccess);
            });
        }

};

}(sg = sg || {}, jQuery));