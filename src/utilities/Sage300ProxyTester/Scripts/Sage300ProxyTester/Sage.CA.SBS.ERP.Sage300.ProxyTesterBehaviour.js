// The MIT License (MIT) 
// Copyright (c) 2024-2025 The Sage Group plc or its licensors.  All rights reserved.
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

    /**
     * @name init
     * @description Script initialization
     * @namespace ProxyTesterUI
     * @public
     */
    init: function () {

        ProxyTesterUI.ViewModel = proxyTesterViewModel;

        ProxyTesterUI.initButtons();
        ProxyTesterUI.initOnFocus();
        ProxyTesterUI.setFormValues();
        ProxyTesterUI.regPayrollEvent();
    },

    regPayrollEvent: function () {
        window.addEventListener('message', (e) => {
            if (e.data && e.data.messagetype === 'ChequeProcessSuccess') {
                alert(`Received message type 'ChequeProcessSuccess' with date ${e.data.data}. Assign date to PDF Date input box.`);
                const isoString = e.data.data.toISOString();
                // Split at the "T" character to get the date part
                const formattedDate = isoString.split("T")[0];
                $('#txtPDFDate').val(formattedDate);
            }
        });
    },

    /**
     * @name setFormValues
     * @description Set values from model to controls
     * @namespace ProxyTesterUI
     * @public
     */
    setFormValues: function () {
        $('#txtUsername').val(ProxyTesterUI.ViewModel.User);
        $('#txtPassword').val(ProxyTesterUI.ViewModel.Password);
        $('#txtCompany').val(ProxyTesterUI.ViewModel.Company);

        $('#txtModule').val(ProxyTesterUI.ViewModel.ModuleId);
        $('#txtController').val(ProxyTesterUI.ViewModel.Controller);
        $('#txtAction').val(ProxyTesterUI.ViewModel.Action);
        $('#txtOptionalParameters').val(ProxyTesterUI.ViewModel.OptionalParameters);
    },

    /**
     * @name setModelValues
     * @description Set values from controls to model
     * @namespace ProxyTesterUI
     * @public
     */
    setModelValues: function () {
        ProxyTesterUI.ViewModel.User = $('#txtUsername').val();
        ProxyTesterUI.ViewModel.Password = $('#txtPassword').val();
        ProxyTesterUI.ViewModel.Company = $('#txtCompany').val();

        ProxyTesterUI.ViewModel.ModuleId = $('#txtModule').val();
        ProxyTesterUI.ViewModel.Controller = $('#txtController').val();
        ProxyTesterUI.ViewModel.Action = $('#txtAction').val();
        ProxyTesterUI.ViewModel.OptionalParameters = $('#txtOptionalParameters').val();
        ProxyTesterUI.ViewModel.PdfFileName = $('#txtFileName').val();
        ProxyTesterUI.ViewModel.PdfFileDate = $('#txtPDFDate').val();

    },

    /**
     * @name initButtons
     * @description Button initialization
     * @namespace ProxyTesterUI
     * @public
     */
    initButtons: function () {

        // Test menu button
        $('#btnMenu').click(function (e) {
            // Set values into the model
            ProxyTesterUI.setModelValues();
            // Build URL (local)
            var url = ProxyTesterUI.ViewModel.ProxyTesterServer + '/Home/GetMenu';
            // Call the AJAX post with callback function to display alert
            ProxyTesterUI.ajaxPost(url, ProxyTesterUI.ViewModel, ProxyTesterUI.displayAlert, ProxyTesterUI.errorMessage);
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
            ProxyTesterUI.ajaxPost(url, ProxyTesterUI.ViewModel, ProxyTesterUI.assignSource, ProxyTesterUI.errorMessage);
            e.preventDefault();
        });

        $('#btnPDF').click(function (e) {
            // Set values into the model
            ProxyTesterUI.setModelValues();
            // Build URL (local)
            var url = ProxyTesterUI.ViewModel.ProxyTesterServer + '/Home/GetPDF';

            ProxyTesterUI.ajaxPost(url, ProxyTesterUI.ViewModel, ProxyTesterUI.assignPDF, ProxyTesterUI.errorMessage);
            e.preventDefault();
        });

        $('#btnPDFDate').on("click", function (e) {
            // Set values into the model
            ProxyTesterUI.setModelValues();
            // Build URL (local)
            var url = ProxyTesterUI.ViewModel.ProxyTesterServer + '/Home/GetPDFFileName';

            ProxyTesterUI.ajaxPost(url, ProxyTesterUI.ViewModel, ProxyTesterUI.assignPDFFileName, ProxyTesterUI.errorMessage);
            e.preventDefault();
        });

        $('#btnMultiplePDF').on("click", function (e) {
            // Set values into the model
            ProxyTesterUI.setModelValues();
            // Build URL (local)
            var url = ProxyTesterUI.ViewModel.ProxyTesterServer + '/Home/GetPDF';
            e.preventDefault();

            const startTime = performance.now();

            for (let i = 0; i < 1000; i++) {
                ProxyTesterUI.ViewModel.PdfFileName = `SamplePaySlip${i}.pdf`;

                ProxyTesterUI.ajaxPost(url, ProxyTesterUI.ViewModel, (e) => { }, ProxyTesterUI.errorMessage);
            }
            const endTime = performance.now();

            alert(`Call to request and download 1000 files took ${endTime - startTime} milliseconds`);

        });

        $('#selectFile').on('change', function (e) {
            $('#txtFileName').val(this.value);
        });
    },

    /**
     * @name initOnFocus
     * @description Form initialization
     * @namespace ProxyTesterUI
     * @public
     */
    initOnFocus: function () {
        $('.form-control').on('focus', function () {
            ProxyTesterUI.resetValidations();
        });
    },

    /**
     * @name displayAlert
     * @description Displays an alert for the menu since binding to an iFrame is silly
     * @namespace ProxyTesterUI
     * @public
     */
    displayAlert: function (data) {
        alert(data);
    },

    /**
     * @name assignSource
     * @description Assigns the source to the iFrame from the proxy result
     * @namespace ProxyTesterUI
     * @public
     */
    assignSource: function (data) {
        $("#ExternalFrame").attr('src', data);
    },

    assignPDF: function (fileName) {
        ///PDFs/SamplePaySlip.pdf
        $("#pdfObject").attr('data', `/PDFs/${fileName}`);
    },

    assignPDFFileName: function (fileNames) {
        let $selectFile = $('#selectFile');
        $selectFile.empty();

        if (fileNames && fileNames.length) {
            let fileNameArray = fileNames.split(",");
            fileNameArray.forEach(function (item, index) {
                $selectFile.append(new Option(item, item));
            });
        }
    },

    /**
 * @name errorMessage
 * @description Displays error message
 * @namespace ProxyTesterUI
 * @public
 */
    errorMessage: function (message) {
        // alert(message.responseText);
        alert("An error was thrown. Check credentials and parameters for validity.");
    },

    /**
     * @name ajaxPost
     * @description Ajax to invoke controller in proxy tester to invoke Proxy in Sage 300
     * @namespace ProxyTesterUI
     * @public
     */
    ajaxPost: function (url, data, successHandler, errorHandler) {
        var dataJson = JSON.stringify(data);
        $.ajaxq("ProxyTester", {
            url: url,
            data: dataJson,
            type: "post",
            async: false,
            dataType: "text",
            contentType: "application/json",
            success: successHandler,
            error: errorHandler
        });
    },

    /**
     * @name resetValidations
     * @description Form reset validations
     * @namespace ProxyTesterUI
     * @public
     */
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
