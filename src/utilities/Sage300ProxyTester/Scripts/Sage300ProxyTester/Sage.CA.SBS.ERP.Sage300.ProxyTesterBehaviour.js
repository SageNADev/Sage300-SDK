// The MIT License (MIT) 
// Copyright (c) 2023 The Sage Group plc or its licensors.  All rights reserved.
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

    FormActionEnum: Object.freeze({
        TEST_PROXYGETMENU: 0,
        TEST_SAGE_WEBSCREEN: 1,
        TEST_PARTNER_WEBSCREEN: 2
    }),

    ModuleTypeEnum: Object.freeze({
        SAGE: 0,
        PARTNER: 1
    }),

    ViewModel: {},

    init: function () {
        let UI = ProxyTesterUI;

        UI.initButtons();
        UI.initTextBoxes();
        UI.initCheckBoxes();
        UI.initRadioButtons();
        UI.initDropDownLists();
        UI.initOnFocus();

        // Final initialization functions
        $('#SageWebScreenGroup').show();
        $('#PartnerWebScreenGroup').hide();
        $('#ModuleTypeSage').prop("checked", true);

        UI.ViewModel = proxyTesterViewModel;

        // Set the initial form values
        UI.setFormValues();
    },

    setFormValues: function () {
        let UI = ProxyTesterUI;

        $('#txtUsername').val(UI.ViewModel.Username);
        $('#txtPassword').val(UI.ViewModel.Password);
        $('#txtCompany').val(UI.ViewModel.CompanyDatabase);

        $('#chkHttps').prop('checked', UI.ViewModel.Https);
        $('#txtServer').val(UI.ViewModel.Sage300Server);
        $('#txtServerPort').val(UI.ViewModel.ServerPort);

        // Set the correct tab (after form submission)
        // Also, if were on tab 2, we need to set the correct
        // module type radio button (Sage or Partner)
        switch (UI.ViewModel.Action) {

            case UI.FormActionEnum.TEST_PROXYGETMENU:
                UI.setActiveTab('1');
                break;

            case UI.FormActionEnum.TEST_SAGE_WEBSCREEN:
            case UI.FormActionEnum.TEST_PARTNER_WEBSCREEN:
                UI.setActiveTab('2');
                UI.setActiveModuleSelector(UI.ViewModel.Action);
                break;
            default:
                break;
        }

        $('#txtModule').val(UI.ViewModel.Module);

        var module = UI.ViewModel.SageModule;
        $('#ddlSageModule').val(module);
        if (module?.length > 0) {
            // Need to ensure these dropdowns have been set to the correct lists
            // since SageModule was specified.
            UI.rebuildCategoryList(module);
            UI.rebuildScreenList(module, UI.ViewModel.Category);

            // Now, set the currently selected values
            $('#ddlCategory').val(UI.ViewModel.Category);
            $('#ddlScreen').val(UI.ViewModel.Screen);
        }

        $('#txtSageOtherParams').val(UI.ViewModel.SageOtherParameters);

        $('#txtPartnerModule').val(UI.ViewModel.PartnerModule);
        $('#txtPartnerOtherParams').val(UI.ViewModel.PartnerOtherParameters);

        if (UI.ViewModel.Url.length > 0) {
            $('#lblUrl').text(UI.ViewModel.Url);
        } else {
            $('#lblUrl').text(UI.ViewModel.UrlDescription);
        }

        if (UI.ViewModel.PublicKey) {
            $.post({
                xhrFields: {
                    responseType: 'blob'
                },
                url: UI.ViewModel.Url,
                beforeSend: (xhr) => {
                    // add authentication headers
                    xhr.setRequestHeader("Credentials", UI.ViewModel.Credentials);
                    xhr.setRequestHeader("PublicKey", UI.ViewModel.PublicKey);
                    xhr.setRequestHeader("IV", UI.ViewModel.IV);
                },
                success: (data) => {
                    // set iframe (WIP)
                    var data_url = URL.createObjectURL(data);
                    $("#ExternalFrame").attr('src', data_url);
                }
            });
        }
        else {
            $("#ExternalFrame").attr('src', UI.ViewModel.Url);
        }
    },

    initTextBoxes: function () {
        $('#txtUsername').val('');
        $('#txtPassword').val('');
        $('#txtCompany').val('');
        $('#txtServer').val('');
        $('#txtServerPort').val('');
        $('#txtModule').val('');
        $('#txtSageOtherParams').val('');
        $('#txtPartnerModule').val('');
        $('#txtPartnerOtherParams').val('');
    },

    initCheckBoxes: function () {
        $('#chkHttps').val(0);
    },

    initRadioButtons: function () {
        // Sage/Partner Radio button click handlers
        $('#ModuleTypeSage').click(function () {
            $('#SageWebScreenGroup').show();
            $('#PartnerWebScreenGroup').hide();
        });

        $('#ModuleTypePartner').click(function () {
            $('#SageWebScreenGroup').hide();
            $('#PartnerWebScreenGroup').show();
        });
    },

    initButtons: function () {
        let UI = ProxyTesterUI;

        $('#btnTestProxyGetMenu').click(function (e) {
            $('#hdnAction').val(UI.FormActionEnum.TEST_PROXYGETMENU);
            $(`#${UI.Constants.FORMNAME}`).submit();
        });

        $('#btnGetPage').click(function (e) {
            var moduleTypeSelector = $('input[name=ModuleType]');
            var selectedModuleType = Number(moduleTypeSelector.filter(":checked").val());
            switch (selectedModuleType) {
                case (UI.ModuleTypeEnum.SAGE):
                    $('#hdnAction').val(UI.FormActionEnum.TEST_SAGE_WEBSCREEN);
                    break;

                case (UI.ModuleTypeEnum.PARTNER):
                    $('#hdnAction').val(UI.FormActionEnum.TEST_PARTNER_WEBSCREEN);
                    break;
                default:
                    break;
            }

            $(`#${UI.Constants.FORMNAME}`).submit();
        });
    },

    initDropDownLists: function () {
        let UI = ProxyTesterUI;

        // Set the initial values
        $('#ddlSageModule').val('');
        $('#ddlCategory').val('');
        $('#ddlScreen').val('');

        // Setup the change handlers
        $("#ddlSageModule").on('change', function (e) {
            var module = e.target.value;
            if (module.length === 0) {
                $('#txtSageOtherParams').val('');
            }
            $('#ddlCategory').empty();
            $('#ddlScreen').empty();
            UI.rebuildCategoryList(module);
        });

        $("#ddlCategory").on('change', function (e) {
            var module = $('#ddlSageModule').val();
            var category = e.target.value;
            if (category.length === 0 ) {
                $('#txtSageOtherParams').val('');
            }
            $('#ddlScreen').empty();
            UI.rebuildScreenList(module, category);
        });

        $("#ddlScreen").on('change', function (e) {
            var screen = e.target.value;
            if (screen.length === 0) {
                $('#txtSageOtherParams').val('');
            }
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

    // Rebuild the Category list
    rebuildCategoryList: function (module) {
        var ddl = $('#ddlCategory');
        ddl.empty();
        let key = module;
        let categories = ProxyTesterUI.ViewModel.ScreenData[key].sort();

        // Add an empty selection
        ddl.append($('<option></option>').val('').html(''));

        $.each(categories, function (val, text) {
            ddl.append($('<option></option>').val(text).html(text));
        });
    },

    // Rebuild the Screen list
    rebuildScreenList: function (module, category) {
        var ddl = $('#ddlScreen');
        ddl.empty();
        let key = module + category;
        let screens = ProxyTesterUI.ViewModel.ScreenData[key].sort();

        // Add an empty selection
        ddl.append($('<option></option>').val('').html(''));

        $.each(screens, function (val, text) {

            let screenUrl = '';
            let newKey = module + category + text;
            if (ProxyTesterUI.ViewModel.ScreenData.hasOwnProperty(newKey)) {
                screenUrl = ProxyTesterUI.ViewModel.ScreenData[newKey][0];
            } else {
                screenUrl = '';
            }
            ddl.append($('<option></option>').val(screenUrl).html(text));
        });
    },

    setActiveTab: function (tab) {
        $('.nav-tabs a[href="#' + tab + '"]').tab('show');
    },

    setActiveModuleSelector: function (action) {
        let UI = ProxyTesterUI;
        switch (action) {
            case UI.FormActionEnum.TEST_SAGE_WEBSCREEN:
                $('#ModuleTypeSage').click();
                break;

            case UI.FormActionEnum.TEST_PARTNER_WEBSCREEN:
                $('#ModuleTypePartner').click();
                break;
            default:
                break;
        }
    },
}

$(function () {
    ProxyTesterUI.init();
});
