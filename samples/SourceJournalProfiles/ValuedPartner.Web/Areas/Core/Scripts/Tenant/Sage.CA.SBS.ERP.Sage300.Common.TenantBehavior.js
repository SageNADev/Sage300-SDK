// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


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