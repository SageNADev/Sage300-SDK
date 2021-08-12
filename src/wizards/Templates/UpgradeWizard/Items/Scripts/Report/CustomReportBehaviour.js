"use strict";
$(function () {

    $(window).on("pagehide", function() {

        var token = $('#hiddenToken').val();
        var data = { "token": token};

        $.ajax({
            type: "post",
            url: "CustomReportViewer.aspx/Release",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false
        });
    });

});