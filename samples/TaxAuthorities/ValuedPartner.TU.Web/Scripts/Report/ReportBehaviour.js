"use strict";
$(function () {

    $(window).on("pagehide", function() {

        var token = $('#hiddenToken').val();
        var sessionId = $('#hiddenSessionId').val();
        var data = { "token": token, "sessionId": sessionId };

        $.ajax({
            type: "post",
            url: "ReportViewer.aspx/Release",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false
        });
    });

});