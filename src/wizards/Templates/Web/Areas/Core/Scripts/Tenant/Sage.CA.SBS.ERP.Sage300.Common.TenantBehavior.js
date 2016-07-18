// Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved.

"use strict";

var tenantSelectionUI = tenantSelectionUI || {};
var tenantSelectionUI = {
    init: function () {
        $("#TenantId").kendoDropDownList();
        tenantSelectionUI.initButtons();
    },
    
    initButtons: function () {
        $("#btnOk").bind('click', function () {
            //Need to add validaion message using annotations.
            if ($("#TenantId").val() === "") {
                return;
            }
            var data;
            var antiforgeryTokenName = $("#antiforgerytoken_holder[data-antiforgerycookiename]").data("antiforgerycookiename");
            var antiforgeryToken = $("#antiforgerytoken_holder input[type='hidden'][name='__RequestVerificationToken']").val();
            var headers = {};
            headers[antiforgeryTokenName] = antiforgeryToken;
            data = { tenantId: $("#TenantId").val() };
            data = JSON.stringify(data);
            tenantSelectionUI.ajaxPost(tenantSelectionElements.urlToSwitchTenant, data, tenantSelectionUI.success, headers);
        });
        $('#btnCancel').bind('click', function () {
            var dropdownlist = $("#TenantId").data("kendoDropDownList");
            dropdownlist.select(0);
        });
    },
    ajaxPost: function (url, data, successHandler, headers) {
        $.ajax({
            type: "Post",
            contentType: 'application/json',
            url: url,
            data: data,
            headers: headers,
            success: successHandler,
            error: function (jqXhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    },
    success: function (result) {
        if (result != null && result.IsSuccess) {
            window.location.href = result.Url;
        }
    },
};

$(function () {
    tenantSelectionUI.init();
});