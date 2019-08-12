// The MIT License (MIT) 
// Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved.
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

/*global receiptResources*/
/*global ko*/
/*global kendo*/
/*global type*/
/*global receiptUI*/
/*global recordStatus*/

function receiptObservableExtension(viewModel, uiMode) {

    var model = viewModel;
    model.UIMode = ko.observable(uiMode);
    model.IsVisibleAllocType = ko.observable(true); 
    model.Data.RateDate = ko.observable(new Date(model.Data.RateDate()));
    model.ReceiptCurrDecimal = ko.observable(model.Data.ReceiptCurrencyDecimals());
    model.AddCostCurrDecimal = ko.observable(model.FuncDecimals());
    model.AdjCostCurrDecimal = ko.observable(model.Data.ReceiptCurrencyDecimals());
    model.IsRequireChecked = ko.observable(false);
    model.IsOptionalFields = ko.observable(false);
    model.lblTotalCost = ko.computed(function () {
        if (model.DisableScreen()) {
            return receiptResources.TotalCost;
        }
        else {
            return parseInt(model.Data.ReceiptType()) === type.RETURN ? receiptResources.TotalReturnCost : receiptResources.TotalCost;
        }
    });


    // Computed property for setting disable mode based on RecordStatus 
    model.ReceiptMode = ko.computed(function () {
        if (model.IsExists()) {
            if (model.Data.Complete()) {
                return type.COMPLETE;
            }
            else if (model.Data.RecordStatus() == recordStatus.ENTERED) {
                return parseInt(model.Data.ReceiptType());
            }
            else {
                return type.RETURN;
            }
        }
        return type.RECEIPT;
    });

    // Computed property for setting disable mode based on FiscalPeriod 
    model.Data.ComputedYearPeriod = ko.computed(function () {
        var fiscalYear = model.Data.FiscalYear();
        return (fiscalYear)? fiscalYear + " - " + window.sg.utls.strPad(model.Data.FiscalPeriod(), 2, "0") : "";
    });

    // Computed property for setting disable mode based on receipt type 
    model.Data.isControlsDisabledOnReadMode = ko.computed(function () { 
        if (model.DisableScreen()) {
            return true;
        }
        if (model.UIMode() === sg.utls.OperationMode.NEW) {
            return false;
        }
        else if (parseInt(model.Data.ReceiptType()) === type.COMPLETE || parseInt(model.Data.ReceiptType()) === type.RETURN || parseInt(model.Data.ReceiptType()) === type.ADJUSTMENT) {
            return true;
        } else {
            return false;
        }
    });

    model.disableAdditionalCost = ko.computed(function () {
        if (model.DisableScreen()) {
            return true;
        } 
        if (model.Attributes() && model.Attributes.AdditionalCost() === false || parseInt(model.Data.ReceiptType()) === type.ADJUSTMENT) { 
            return false; 
        } else {
            return true;
        }
    }); 
     
    // Computed property for setting disable mode based on receipt type if only complete
    model.IsMultiCurrAndReturn = ko.computed(function () {
        var isReturnCheck = (parseInt(model.Data.ReceiptType()) === type.RETURN) ? false : true;
        return ((model.IsMulticurrency() === true && isReturnCheck) || model.DisableScreen()) ? true : false;
    });

    // Computed property for setting disable mode based on UIMode  
    model.IsDisableOnNewMode = ko.computed(function () {
        return (model.UIMode() === sg.utls.OperationMode.NEW) ? true : false;
    });

    // Computed property for setting disable mode based on receipt type if only complete
    model.IsVisibleOnMode = ko.computed(function () {
        var ctrls = ["#Data_ReceiptType", "#Data_AdditionalCostAllocationType"];
        var isVisible = false;
        $.each(ctrls, function (i, field) {
            var ctrl = $(field).data("kendoDropDownList");
            if (ctrl) {

                //Record status is posted show the control, else hide it. 
                if (model.UIMode() === sg.utls.OperationMode.NEW || (model.Data.RecordStatus() != recordStatus.POSTED && parseInt(model.Data.ReceiptType()) === type.RECEIPT) || (parseInt(model.Data.ReceiptType()) === type.COMPLETE && i == 1)) {
                    ctrl.wrapper.hide();
                    if (i != 1) isVisible = false;
                    if (i === 1) model.IsVisibleAllocType(false);
                }
                else if (model.DisableScreen()) {
                    ctrl.wrapper.show();
                    ctrl.enable(false);
                    isVisible = false;
                }

                else if (parseInt(model.Data.ReceiptType()) === type.RETURN && i == 0) {
                    ctrl.wrapper.show();
                    ctrl.enable(model.Data.RecordStatus() !== recordStatus.ENTERED);
                    isVisible = true;
                }
                else if (parseInt(model.Data.ReceiptType()) === type.ADJUSTMENT && i == 0) {
                    ctrl.wrapper.show();
                    ctrl.enable(model.Data.RecordStatus() !== recordStatus.ENTERED);
                    isVisible = true;
                }
                else if (parseInt(model.Data.ReceiptType()) === type.COMPLETE && i == 0) {
                    ctrl.wrapper.show();
                    ctrl.enable(false);
                    isVisible = true;
                }
                else if (parseInt(model.Data.ReceiptType()) === type.ADJUSTMENT && i == 1) {
                    ctrl.wrapper.show();
                    ctrl.enable(false);
                    model.IsVisibleAllocType(true);
                    isVisible = true;
                }
                else {
                    ctrl.wrapper.show();
                    ctrl.enable(true);
                    model.IsVisibleAllocType(true);
                    isVisible = true;
                }
            }
        });
        return isVisible;
    });
   
    // Computed property for setting the mode
    model.IsReturn = ko.computed(function () {
        return (parseInt(model.Data.ReceiptType()) === type.RETURN) ? false : true;
    });

    // Computed property for setting disable mode based on RecordStatus if only complete
    model.IsPosted = ko.computed(function () {

        return (model.Data.RecordStatus() === recordStatus.POSTED || parseInt(model.Data.ReceiptType()) != type.RECEIPT || model.DisableScreen()) ? true : false;
    });

    // Computed property for setting disable mode based on receipt type if only complete
    model.IsDisableOnlyComplete = ko.computed(function () {
        return parseInt(model.Data.ReceiptType()) === type.COMPLETE || model.DisableScreen() ? true : false;
    });

    //Subscribe for change event
    model.Data.ReceiptType.subscribe(function (value) {
        receiptUI.showHideColumns(value);
    });

    // Computed property for setting disable mode based on isControlsDisabledOnReadMode if only complete
    receiptUI.isEditable = ko.computed(function () {
        return (model.Data.isControlsDisabledOnReadMode()) ? false : true;
    });

    // Computed property for setting disable mode based on OptionalFields if only complete
    model.Data.IsOptionalFields = ko.computed(function () {
        return (parseInt(model.Data.OptionalFields()) > 0) ? true : false;
    });

    // Computed property for setting disable mode based on RequireLabels if only complete
    model.Data.IsRequireLabel = ko.computed(function () {
        if (model.UIMode() === sg.utls.OperationMode.LOAD || model.UIMode() === sg.utls.OperationMode.NEW) {
            var isChecked = (parseInt(model.Data.RequireLabels()) == 1) ? true : false;
            model.IsRequireChecked(isChecked);
        }
        return (parseInt(model.Data.RequireLabels()) == 1) ? true : false;
    });
    
    // Computed property for setting disable mode based on ReceiptCurrency if only complete
    model.IsFuncCurrency = ko.computed(function () {
        if (model.Data.ReceiptCurrency() == model.FuncCurrency())
            return true;
        else if (model.Data.isControlsDisabledOnReadMode())
            return true;
        return false;
    }); 
      
    // Computed property for setting disable mode based on ReceiptCurrency if only complete
    model.IsFuncCurrencyDisable = ko.computed(function () {
        if (model.Data.ReceiptCurrency() == model.FuncCurrency())
            return true; 
        return false;
    });

    // Computed property for setting disable mode based on receipt type 
    model.Data.disableExchangeRate = ko.computed(function () {
        if (model.DisableScreen()) {
            return true;
        } 
        if (model.UIMode() == sg.utls.OperationMode.NEW) {
            return false;
        }
        else if (parseInt(model.Data.ReceiptType()) === type.COMPLETE) {
            return true;
        } else {
            return false;
        }
    });



    //Subscribe for change event
    model.Data.RequireLabels.subscribe(function (value) {
        return value;
    });

    model.Data.IsOptionalFields.subscribe(function (value) {
        model.IsOptionalFields(value);
        return value;
    });

    //Subscribe for change event
    model.IsRequireChecked.subscribe(function (value) {
        if (model.UIMode() === sg.utls.OperationMode.SAVE || model.UIMode() === sg.utls.OperationMode.NEW) {
            model.Data.RequireLabels(value ? 1 : 0);
        }
        return value;
    });
    
    //Computed property for TotalCostCurrency 
    model.TotalCostCurrency = ko.computed(function () {
        return model.Data.AdditionalCostCurrency();
    });

    //Computed property for ExtendedCostCurrency 
    model.ExtendedCostCurrency = ko.computed(function () {
        return model.Data.ReceiptCurrency();
    });
     
    
    //Subscribe for change event
    model.ExtendedCostCurrency.subscribe(function (value) {
        return value;
    });

    // IsDisableRecCurr based on VendorExists
    model.IsDisableRecCurr = ko.computed(function () {
        return (model.Data.isControlsDisabledOnReadMode()) ? true : (model.Data.VendorExists()) ? true : false;
    });

    //Computed property for setting disable mode based on receipt type
    model.IsDisableAddlCost = ko.computed(function () {
        return parseInt(model.Data.ReceiptType()) === type.COMPLETE || parseInt(model.Data.ReceiptType()) === type.RETURN ? true : false;
    });

    //Computed property for setting disable mode of save/post button based on receipt type
    model.IsDisableSaveBtn = ko.computed(function () {
        return (model.ReceiptMode() === type.RECEIPT || model.ReceiptMode() === type.RETURN || model.ReceiptMode() === type.ADJUSTMENT) && parseInt(model.Data.ReceiptType()) !== type.COMPLETE && !model.DisableScreen() ? false : true;
    });

    //Computed property for setting disable mode of post button based on receipt type
    model.IsDisablePostBtn = ko.computed(function () {
        return (parseInt(model.Data.ReceiptType()) === type.RECEIPT || parseInt(model.Data.ReceiptType()) === type.RETURN || parseInt(model.Data.ReceiptType()) === type.ADJUSTMENT || model.ReceiptMode() !== type.COMPLETE)
            && !model.DisableScreen() ? false : true;
    });
      
    //Computed property for setting  disable mode of delete button based on receipt type
    model.IsDisableDelBtn = ko.computed(function () {
        if (parseInt(model.Data.ReceiptType()) !== type.RECEIPT)
            return true;
        if (model.UIMode() === sg.utls.OperationMode.NEW)
            return true;
        if (model.IsExists())
            return false;
        else
            return true;
        return false;
    });  

    //Computed property for TotalCost 
    model.TotalCost = ko.computed(function () { 
        var decimal = 0;
        if (model.Data.AdditionalCostCurrency() != model.Data.ReceiptCurrency()) {

            if (model.lblTotalCost() === receiptResources.TotalReturnCost && parseInt(model.Data.ReceiptType()) === type.RETURN ) {
                decimal = model.Data.ReceiptCurrencyDecimals();
            }
            else {
                decimal = model.FuncDecimals();
            }
        }
        else {
            decimal = model.FuncDecimals();
        }
        var formatted;
        if (model.DisableScreen() == true) {
            formatted = kendo.toString(parseFloat(model.Data.TotalCostReceiptAdditional()), "n" + decimal);
        }
        else {
            if (parseInt(model.Data.ReceiptType()) === type.RECEIPT || parseInt(model.Data.ReceiptType()) === type.COMPLETE) {
                formatted = kendo.toString(parseFloat(model.Data.TotalCostReceiptAdditional()), "n" + decimal);
            } else if (parseInt(model.Data.ReceiptType()) === type.RETURN) {
                formatted = kendo.toString(parseFloat(model.Data.TotalReturnCost()), "n" + decimal);
            } else if (parseInt(model.Data.ReceiptType()) === type.ADJUSTMENT) {
                formatted = kendo.toString(parseFloat(model.Data.TotalAdjCostReceiptAddl()), "n" + decimal);
            }
        }
        return formatted;
    });
    
    model.TotalExtendedCostCurrency = ko.computed(function () {
        return (parseInt(model.Data.ReceiptType()) === type.RETURN && !model.DisableScreen()) ? model.Data.ReceiptCurrency() : model.Data.AdditionalCostCurrency();
    });


    //Computed property for TotalExtendedCost 
    model.TotalExtendedCost = ko.computed(function () {
        var decimal = model.Data.ReceiptCurrencyDecimals();
        var formatted; 
        if (parseInt(model.Data.ReceiptType()) === type.RECEIPT || parseInt(model.Data.ReceiptType()) === type.COMPLETE) {
            if (model.IsMulticurrency()) {
                formatted = kendo.toString(parseFloat(model.Data.TotalExtendedCostSource()), "n" + decimal);
            }
            formatted = kendo.toString(parseFloat(model.Data.TotalExtendedCostSource()), "n" + decimal);
        } else if (parseInt(model.Data.ReceiptType())=== type.RETURN) {
            formatted = kendo.toString(parseFloat(model.Data.TotalExtendedCostSource()), "n" + decimal);
        } else if (parseInt(model.Data.ReceiptType()) === type.ADJUSTMENT) {
            if (model.IsMulticurrency()) {
                formatted = kendo.toString(parseFloat(model.Data.TotalExtendedCostAdjusted()), "n" + decimal);
            }
        }
        return formatted;
    });

    model.Data.AdditionalCost.subscribe(function(value) {
        return value;
    });
    
    model.TotalCost.subscribe(function (value) {
        return value;
    });
    model.TotalExtendedCost.subscribe(function (value) {
        return value;
    });

    model.Data.ExchangeRate.subscribe(function (value) {
        receiptUI.initNumericTextBox();
        return value;
    });

    //Subscribe for change event of ReceiptType
    model.Data.ReceiptType.subscribe(function (value) {
        return value;
    });
    
       model.Data.IsTotalCostReceiptAdditional = ko.computed(function () {
           if (receiptUI.receiptModel.Data.ReceiptType() != "2" && receiptUI.receiptModel.Data.ReceiptType() != "3") {
            return true;
        } else {
            return false;
        }
    });

    model.Data.IsTotalReturnCost = ko.computed(function () {
        if (receiptUI.receiptModel.Data.ReceiptType() == "2") {
            return true;
        } else {
            return false;
        }
    });
    model.Data.IsTotalAdjustmentCost = ko.computed(function () {
        if (receiptUI.receiptModel.Data.ReceiptType() == "3") {
            return true;
        } else {
            return false;
        }
    });
}