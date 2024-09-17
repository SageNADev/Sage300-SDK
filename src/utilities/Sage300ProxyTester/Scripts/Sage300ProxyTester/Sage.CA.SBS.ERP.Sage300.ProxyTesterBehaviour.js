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

    Constants: Object.freeze({
        FORMNAME: 'TheForm',
    }),

    ViewModel: {},

    init: function () {
        let UI = ProxyTesterUI;

        UI.initButtons();
        UI.initOnFocus();

        UI.ViewModel = proxyTesterViewModel;

        // Set the initial form values
        UI.setFormValues();
    },

    setFormValues: function () {
        let UI = ProxyTesterUI;

        $('#txtUsername').val(UI.ViewModel.User);
        $('#txtPassword').val(UI.ViewModel.Password);
        $('#txtCompany').val(UI.ViewModel.Company);

        $('#txtServer').val(UI.ViewModel.Server);

        $('#txtModule').val(UI.ViewModel.ModuleId);
        $('#txtController').val(UI.ViewModel.Controller);
        $('#txtAction').val(UI.ViewModel.Action);

        $('#txtOptionalParameters').val(UI.ViewModel.OptionalParameters);

        $('#txtPublicKeyUrl').text(UI.ViewModel.PublicKeyUrl);
        $('#txtLoginUrl').text(UI.ViewModel.LoginUrl);
        $('#txtValidTokenUrl').text(UI.ViewModel.IsValidTokenUrl);
        $('#txtMenuUrl').text(UI.ViewModel.MenuUrl);
        $('#txtScreenUrl').text(UI.ViewModel.ScreenUrl);
        $('#hdnToken').val(UI.ViewModel.Token);

    //    if (UI.ViewModel.PublicKey) {
    //        $.post({
    //            xhrFields: {
    //                responseType: 'blob'
    //            },
    //            url: UI.ViewModel.Url,
    //            beforeSend: (xhr) => {
    //                // add authentication headers
    //                xhr.setRequestHeader("Credentials", UI.ViewModel.Credentials);
    //                xhr.setRequestHeader("PublicKey", UI.ViewModel.PublicKey);
    //                xhr.setRequestHeader("IV", UI.ViewModel.IV);
    //            },
    //            success: (data) => {
    //                // set iframe (WIP)
    //                var data_url = URL.createObjectURL(data);
    //                $("#ExternalFrame").attr('src', data_url);
    //            }
    //        });
    //    }
    //    else {
    //        $("#ExternalFrame").attr('src', UI.ViewModel.Url);
    //    }
    },

    initButtons: function () {
        let UI = ProxyTesterUI;

        $('#btnMenu').click(function (e) {
            $('#hdnTestAction').val("Menu");
            $(`#${UI.Constants.FORMNAME}`).submit();
        });

        $('#btnScreen').click(function (e) {
            $('#hdnTestAction').val("Screen");
            $(`#${UI.Constants.FORMNAME}`).submit();
        });
    },

    initOnFocus: function () {
        let UI = ProxyTesterUI;
        $('.form-control').on('focus', function () {
            UI.resetValidations();
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
