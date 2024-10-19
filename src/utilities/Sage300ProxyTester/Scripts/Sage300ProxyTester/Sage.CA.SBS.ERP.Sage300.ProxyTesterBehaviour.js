// The MIT License (MIT) 
// Copyright (c) 2024 The Sage Group plc or its licensors.  All rights reserved.
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

var ProxyTesterUI = ProxyTesterUI || {};
ProxyTesterUI = {

    ViewModel: {},

    init: function () {

        ProxyTesterUI.ViewModel = proxyTesterViewModel;

        ProxyTesterUI.initButtons();
        ProxyTesterUI.initOnFocus();
        ProxyTesterUI.setFormValues();
    },

    setFormValues: function () {
        $('#txtUsername').val(ProxyTesterUI.ViewModel.User);
        $('#txtPassword').val(ProxyTesterUI.ViewModel.Password);
        $('#txtCompany').val(ProxyTesterUI.ViewModel.Company);

        $('#txtModule').val(ProxyTesterUI.ViewModel.ModuleId);
        $('#txtController').val(ProxyTesterUI.ViewModel.Controller);
        $('#txtAction').val(ProxyTesterUI.ViewModel.Action);
        $('#txtOptionalParameters').val(ProxyTesterUI.ViewModel.OptionalParameters);
    },

    setModelValues: function () {
        ProxyTesterUI.ViewModel.User = $('#txtUsername').val();
        ProxyTesterUI.ViewModel.Password = $('#txtPassword').val();
        ProxyTesterUI.ViewModel.Company = $('#txtCompany').val();

        ProxyTesterUI.ViewModel.ModuleId = $('#txtModule').val();
        ProxyTesterUI.ViewModel.Controller = $('#txtController').val();
        ProxyTesterUI.ViewModel.Action = $('#txtAction').val();
        ProxyTesterUI.ViewModel.OptionalParameters = $('#txtOptionalParameters').val();
    },

    initButtons: function () {
        // Test menu button
        $('#btnMenu').click(function (e) {
            // Set values into the model
            ProxyTesterUI.setModelValues();
            // Build URL (local)
            var url = ProxyTesterUI.ViewModel.ProxyTesterServer + '/Home/GetMenu';
            // Call the AJAX post with callback function to assign to iFrame
            ProxyTesterUI.assignSource('about:blank');
            ProxyTesterUI.ajaxPost(url, ProxyTesterUI.ViewModel, ProxyTesterUI.assignSource);
            e.preventDefault();
        });

        // Test screen button
        $('#btnScreen').click(function (e) {
            // Set values into the model
            ProxyTesterUI.setModelValues();
            // Build URL (local)
            var url = ProxyTesterUI.ViewModel.ProxyTesterServer + '/Home/GetScreen';
            // Call the AJAX post with callback function to assign to iFrame
            ProxyTesterUI.assignSource('about:blank');
            ProxyTesterUI.ajaxPost(url, ProxyTesterUI.ViewModel, ProxyTesterUI.assignSource);
            e.preventDefault();
        });
    },

    initOnFocus: function () {
        $('.form-control').on('focus', function () {
            ProxyTesterUI.resetValidations();
        });
    },

    assignSource: function (data) {
        $("#ExternalFrame").attr('src', data);
    },

    ajaxPost: function (url, data, successHandler) {
        var dataJson = JSON.stringify(data);
        $.ajaxq("ProxyTester", {
            url: url,
            data: dataJson,
            type: "post",
            async: false,
            dataType: "text",
            contentType: "application/json",
            success: successHandler
        });
    },

    resetValidations: function() {
        // Removes validation from input-fields
        $('.input-validation-error').addClass('input-validation-valid');
        $('.input-validation-error').removeClass('input-validation-error');

        // Removes validation message after input-fields
        $('.field-validation-error').html("");
        $('.field-validation-error').addClass('field-validation-valid');
        $('.field-validation-error').removeClass('field-validation-error');

        // Removes validation summary 
        $('.validation-summary-errors').addClass('validation-summary-valid');
        $('.validation-summary-errors').removeClass('validation-summary-errors');
    },

}

$(function () {
    ProxyTesterUI.init();
});
