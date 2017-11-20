﻿/* Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved. */

"use strict";

var loginUI = loginUI || {};
loginUI = {
    model: {},
    companyList: [],
    ddCompanies: [],
    companiesSecureMap: {},
    pageInit: true,
    userIdUpdated: true,
    enterToLogin: false,

    // Main routine
    init: function (model) {
        if (model && model.ForAdmin) {
            $(document.body).addClass('admin');
            $("#passwordDiv").show();
            $("#txtUserId").val('ADMIN');
            $("#txtUserId").prop("disabled", true);
            loginUI.companyList = model.Companies;
        }

        
        // Allow up and down to also alter UI just as change event does
        // NOTE: up/down is regardless of change since change fires AFTER 
        // data has changed
        $("#CompanyId").kendoDropDownList().bind("change keyup", function (e) {
            // Show the password field for the seleted company?
            loginUI.showPasswordField($(this).val());
        });

        $("#SystemId").kendoDropDownList().bind("change", function (e) {
            loginUI.updateCompanylist();
        });

        if (model && model.ForAdmin) {
            loginUI.ddCompanies = $('#CompanyId').data("kendoDropDownList").dataSource.data();
            loginUI.updateCompanylist();
        }

        loginUI.initEvents();
        loginUIUnities.initCompaniesDropDown();
        if (!model.CompanyListEnabled) {
            for (var index in model.Companies) {
                var company = model.Companies[index];
                loginUI.companiesSecureMap[company.Id] = company.IsSecurityEnabled;
            }
        }
        loginUI.initialLoad(model);


        // Set focus
        $("#txtUserId").focus();
    },

    // update company drop down list 
    updateCompanylist: function() {
        var ddCompany = $('#CompanyId').data("kendoDropDownList");
        var ddSystem = $("#SystemId").data("kendoDropDownList");
        var sysCompanies;
        if (loginUI.model.CompanyListEnabled && loginUI.model.CompanyListEnabled()) {
            sysCompanies = loginUI.companyList.filter(function(i) { return i.SystemId == ddSystem.text() });
        } else {
            //improve it in future
            sysCompanies = loginUI.companyList.filter(function (i) { return i.SystemId == $("#SystemId").val() });
        }
        var listCompanies = sysCompanies.map(function(i) { return i.Id});
        //var ddList = ddCompany.dataSource.data();
        var selectCompanys = loginUI.ddCompanies.filter(function (i) {
            return listCompanies.indexOf(i.value) > -1;
        });
        ddCompany.dataSource.data(selectCompanys);
        ddCompany.select(0);
    },
    // Init events
    initEvents: function () {
        // Sign in button
        $("#btnLogin").bind('click', function () {
            var company = loginUI.model.ForAdmin() && loginUI.model.CompanyListEnabled() ? $("#SystemId").val() : $("#CompanyId").val();

            var data = {
                company: company,
                userId: $("#txtUserId").val(),
                password: $("#txtPassword").val(),
                forAdmin: $("#loginHeader2").is(":visible"),
                companies: ko.mapping.toJS(loginUI.model.Companies())
            };
            loginRepository.login(data, loginUICallback.loginResult);
        });

        /**
         * Register the event for input box of password 
         */
        $("#txtPassword").change(function () {
            if (!loginUI.model.CompanyListEnabled()) {
                return;
            }
            var data = {
                userId: $("#txtUserId").val(),
                password: $("#txtPassword").val(),
            };
            loginRepository.getUserListOfCompanies(data).then(loginUICallback.getCompaniesSuccess, loginUICallback.getCompaniesFailedWithErrorMsg).then(function () {
                    if (loginUI.enterToLogin) {
                        loginUI.enterToLogin = false;
                        $("#btnLogin").click();
                    }
                });
        });

        /**
         * Register the event for input context changed at first time
         */
        $("#txtUserId").on("input",
            function () {
                if ($("#txtPassword").val() && loginUI.userIdUpdated) {
                    loginUI.userIdUpdated = false;
                    $("#txtPassword").val("");
                    setTimeout(function () {
                        if (!loginUI.model.CompanyListEnabled()) {
                            return;
                        }
                        var pwd = $("#txtPassword").val();
                        var data = {
                            userId: $("#txtUserId").val(),
                            password: pwd,
                        };
                        loginRepository.getUserListOfCompanies(data).then(loginUICallback.getCompaniesSuccess, loginUICallback.updatedUserId);
                    });
                }
            })

        /**
         * Register the event for input box of user name
         */
        $("#txtUserId").change(function () {
            $("#txtPassword").val("");
            loginUI.userIdUpdated = true;
            setTimeout(function () {
                if (!loginUI.model.CompanyListEnabled()) {
                    return;
                }
                var pwd = $("#txtPassword").val();
                var data = {
                    userId: $("#txtUserId").val(),
                    password: pwd,
                };
                var ret = loginRepository.getUserListOfCompanies(data);
                //The password should always be cleared, keep the logic here for feature modify
                if (pwd) {
                    ret.then(loginUICallback.getCompaniesSuccess, loginUICallback.getCompaniesFailedWithErrorMsg);
                }
                else {
                    ret.then(loginUICallback.getCompaniesSuccess, loginUICallback.getCompaniesFailedWithoutErrorMsg).then(function () {
                        if (loginUI.enterToLogin) {
                            loginUI.enterToLogin = false;
                            $("#btnLogin").click();
                        }
                    });
                }
            });
        });

        // Allow ENTER to proceed with sign in from any control with the
        // exception of Change Password link which displays Change Password dialog
        $(document).keypress(function (event) {
            // Get key code and evaluate
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                // <ENTER>. Determine routing
                var currentControl = event.target.id;
                if (currentControl === "lnkChangePassword") {
                    loginUI.displayPassword();
                    return false;
                } else if (currentControl === "") {
                    $("#btnLogin").click();
                    return false;
                } else if (currentControl === "txtPasswordUserId" ||
                    currentControl === "txtPasswordOld" ||
                    currentControl === "txtPasswordNew" ||
                    currentControl === "txtPasswordConfirm") {
                    loginUI.changePassword();
                    return false;
                } else if (currentControl === "txtUserId" ||
                currentControl === "txtPassword") {
                    if (!loginUI.model.CompanyListEnabled()) {
                        $("#btnLogin").click();
                    } else {
                        loginUI.enterToLogin = true;
                        $("#" + currentControl).trigger("change");
                    }
                    return false;
                }
                else {
                    return true;
                }
            }
        });
    },

    // Sign in
    signIn: function() {
        // Bundle data and sent to controller
        var data = {
            company: $("#CompanyId").val(),
            userId: $("#txtUserId").val(),
            password: $("#txtPassword").val(),
            forAdmin: $("#loginHeader2").is(":visible"),
            companies: ko.mapping.toJS(loginUI.model.Companies())
        };
        loginRepository.login(data, loginUICallback.loginResult);
    },

    // Clear success
    clearSuccess: function () {
        $("#success").stop(true, true).hide();
        $("#success").empty();
    },

    // Clear message
    clearMessage: function () {
        $("#injectedOverlayTransparent").remove();
        $("#message").empty();
    },

    // Show message
    showMessage: function (html, jsonResult) {
        loginUI.clearSuccess();
        loginUI.clearMessage();
        $("#frmOnPremiseLogin").keydown(false);

        $("#message").html(html);
        $("#message").parent().append("<div id='injectedOverlayTransparent' class='k-overlay k-overlay-transparent' ></div>");
        $("#message").show();

        // Add Event
        $("#btnMessage").on('click', jsonResult, loginUI.hideMessage);

        // Set focus
        $("#btnMessage").focus();
    },

    // Hide message
    hideMessage: function (jsonResult) {
        // Remove Event
        $("#injectedOverlayTransparent").remove();
        $("#btnMessage").off('click', loginUI.hideMessage);
        $("#frmOnPremiseLogin").unbind("keydown");
        $("#txtPassword").val("").focus();

        $("#message").hide();

        // Determine if redirection is required
        if (jsonResult != null) {
            if (jsonResult.data.ChangePassword) {
                // Re-direct to Change Password dialog
                loginUI.displayPassword();
            } 
        }

    },

    // Clear password
    clearPassword: function () {
        $("#changePassword").empty();
    },

    // Show password
    showPassword: function (html) {
        loginUI.clearSuccess();
        loginUI.clearPassword();

        $("#changePassword").html(html);
        $("#changePassword").show();

        // Default userid from login screen
        $("#txtPasswordUserId").val($("#txtUserId").val());

        // Add Events
        $("#btnPasswordCancel").on('click', loginUI.hidePassword);
        $("#btnPasswordClose").on('click', loginUI.hidePassword);
        $("#btnPasswordOk").on('click', loginUI.changePassword);

        // Set focus
        $("#txtPasswordUserId").focus();
    },

    // Hide password
    hidePassword: function () {
        // Remove Events
        $("#btnPasswordCancel").off('click', loginUI.hidePassword);
        $("#btnPasswordClose").off('click', loginUI.hidePassword);

        $("#changePassword").hide();
    },

    // Change Password
    changePassword: function () {
        // Remove Event
        $("#btnPasswordOk").off('click', loginUI.changePassword);

        var data = {
            userId: $("#txtPasswordUserId").val(),
            oldPassword: $("#txtPasswordOld").val(),
            newPassword: $("#txtPasswordNew").val(),
            confirmPassword: $("#txtPasswordConfirm").val()
        };
        loginRepository.changePassword(data, loginUICallback.changePasswordResult);
    },

    // Resume Login
    resumeLogin: function () {
        // Remove Events
        $("#btnPasswordExpiresNo").off('click', loginUI.resumeLogin);

        $("#changePassword").hide();

        var data = {
            company: $("#CompanyId").val(),
            userId: $("#txtUserId").val(),
            password: $("#txtPassword").val(),
            forAdmin: $("#loginHeader2").is(":visible"),
            companies: ko.mapping.toJS(loginUI.model.Companies())
        };
        loginRepository.login(data, loginUICallback.loginResult);
    },

    // Show password expires
    showPasswordExpires: function (html) {
        loginUI.clearSuccess();
        loginUI.clearPassword();

        $("#changePassword").html(html);
        $("#changePassword").show();

        // Add Events
        $("#btnPasswordExpiresYes").on('click', loginUI.hidePasswordExpires);
        $("#btnPasswordExpiresClose").on('click', loginUI.hidePasswordExpires);
        $("#btnPasswordExpiresNo").on('click', loginUI.resumeLogin);

        // Set focus
        $("#btnPasswordExpiresYes").focus();
    },

    // Hide password expires
    hidePasswordExpires: function () {
        // Remove Events
        $("#btnPasswordExpiresYes").off('click', loginUI.hidePasswordExpires);
        $("#btnPasswordExpiresClose").off('click', loginUI.hidePasswordExpires);

        $("#changePassword").hide();

        // Clear resume login flag
        loginRepository.clear(null, null);

        // Display Change Password Dialog
        loginUI.displayPassword();
    },

    // Maps data to controls from model
    initialLoad: function (result) {
        if (result.ForAdmin) {
            result.UserId = "ADMIN";
        }
        loginUI.model = ko.mapping.fromJS(result);
        ko.applyBindings(loginUI.model);

        var company = loginUI.model.Company();

        // Select company in list
        if (!loginUI.model.ForAdmin()) {
            $("#CompanyId").data('kendoDropDownList').value(company);
        }
        // Show the password field for the seleted company?
        loginUI.showPasswordField(company);
    },

    // Determines password visibility
    showPasswordField: function (selectedCompany) {
        if (loginUI.model.ForAdmin() || !selectedCompany) {
            return;
        }
        if (!loginUI.model.CompanyListEnabled()) {
            loginUI.companiesSecureMap[selectedCompany] ? $("#passwordDiv").show() : $("#passwordDiv").hide();
            return;
        }
        
        if (loginUI.companiesSecureMap[selectedCompany] && loginUI.model.SecuredCompanyExists()) {
            $("#passwordDiv").show();
        }
        else if (loginUI.pageInit && loginUI.model.SecuredCompanyExists()) {
            $("#passwordDiv").show();
        }
        else {
            $("#passwordDiv").hide();
        }
    },

    // Display error/warning message(s)
    displayMessage: function (jsonResult) {

        // Build HTML
        var html = "<div class='modal-msg k-window-content'>";

        if (jsonResult.Priority == 2) {
            html = html + "<div class='message-control multiWarn-msg'>";
        } else {
            html = html + "<div class='message-control multiError-msg'>";
        }

        html = html + "<div class='title'>";

        if (jsonResult.Priority == 2) {
            html = html + "<span class='icon multiWarn-icon'></span>";
            html = html + "<h3 id='dialogConfirmation_header'>" + loginResources.warningMessageTitle + "</h3>";
        } else {
            html = html + "<span class='icon multiError-icon'></span>";
            html = html + "<h3 id='dialogConfirmation_header'>" + loginResources.errorMessageTitle + "</h3>";
        }

        html = html + "</div>";
        html = html + "<div class='msg-content'>";
        html = html + "<p>" + jsonResult.Message + "</p>";
        html = html + "<div class='button-group right'>";
        html = html + "<input id='btnMessage' class='btn btn-primary' type='button' value= " + loginResources.OkButton + "></input>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "</div>";
        

        // Add html, event and show message
        loginUI.showMessage(html, jsonResult);
    },

    // Change Password
    displayPassword: function () {

        // Build HTML
        var html = "<div class='modal modal-wide'>";
        html = html + "<div class='wrapper-change-password'>";
        html = html + "<div class='k-window-titlebar k-header'>";
        html = html + "<span class='k-window-title'>" + loginResources.ChangePasswordTitle + "</span>";
        html = html + "<div class='k-window-actions'>";
        html = html + "<a id='btnPasswordClose' role='button' href='#' class='k-window-action k-link'>";
        html = html + "<span role='presentation' class='k-icon k-i-close'>Close</span>";
        html = html + "</a>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "<div class='k-window-content k-content'>";
        html = html + "<div class='form-group'>";
        html = html + "<div class='input-group'>";
        html = html + "<label>" + loginResources.UserIdTitle + "</label>";
        html = html + "<input id='txtPasswordUserId' type='text' value='' class='medium txt-upper'>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "<div class='form-group'>";
        html = html + "<div class='input-group'>";
        html = html + "<label>" + loginResources.OldPasswordTitle + "</label>";
        html = html + "<input id='txtPasswordOld' type='password' class='medium txt-upper' maxlength='64'>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "<div class='form-group'>";
        html = html + "<div class='input-group'>";
        html = html + "<label>" + loginResources.NewPasswordTitle + "</label>";
        html = html + "<input id='txtPasswordNew' type='password' class='medium txt-upper' maxlength='64'>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "<div class='form-group'>";
        html = html + "<div class='input-group'>";
        html = html + "<label>" + loginResources.ConfirmNewPasswordTitle + "</label>";
        html = html + "<input id='txtPasswordConfirm' type='password' class='medium txt-upper' maxlength='64'>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "<div class='button-group right'>";
        html = html + "<input id='btnPasswordCancel' class='btn btn-secondary' type='button' value=" + loginResources.CancelButton + " />";
        html = html + "<input id='btnPasswordOk' class='btn btn-primary' type='button' value=" + loginResources.OkButton + " />";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "</div>";

        // Add html, events and show password
        loginUI.showPassword(html);
    },

    // Password Expires
    displayPasswordExpires: function (jsonResult) {

        // Build HTML
        var html = "<div class='modal-msg k-window-content'>";

        if (jsonResult.Priority == 2) {
            html = html + "<div class='message-control multiWarn-msg'>";
        } else {
            html = html + "<div class='message-control multiError-msg'>";
        }

        html = html + "<div class='title'>";

        if (jsonResult.Priority == 2) {
            html = html + "<span class='icon multiWarn-icon'></span>";
            html = html + "<h3 id='dialogConfirmation_header'>" + loginResources.warningMessageTitle + "</h3>";
        } else {
            html = html + "<span class='icon multiError-icon'></span>";
            html = html + "<h3 id='dialogConfirmation_header'>" + loginResources.errorMessageTitle + "</h3>";
        }

        html = html + "</div>";
        html = html + "<div class='msg-content'>";
        html = html + "<p>" + jsonResult.Message + "</p>";
        html = html + "<div class='button-group right'>";
        html = html + "<input id='btnPasswordExpiresNo' class='btn btn-secondary' type='button' value=" + loginResources.NoButton + " />";
        html = html + "<input id='btnPasswordExpiresYes' class='btn btn-primary' type='button' value=" + loginResources.YesButton + " />";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "</div>";
        html = html + "</div>";

        // Add html, events and show password
        loginUI.showPasswordExpires(html);
    }
};

var loginUIUnities = {
    /**
     * Initialize company drop down list
     * @returns {} 
     */
    initCompaniesDropDown: function() {
        $('#CompanyId').kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
        });
    },

    bindSysCompaniesDropDown: function (companyList, selectedCompanyId) {
        $('#SystemId').kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
        });
        loginUI.companyList = companyList;
        loginUI.companiesSecureMap = {};
        loginUI.model.Companies = ko.mapping.fromJS(companyList, { arrayUpdate: true, arrayInsert: true });

        var sysCompanies = companyList.map(function(company) {
            return {
                systemId: company.SystemId,
                companyId: company.Id
            };
        });
        var ddCompaniesList = sysCompanies.map(function (company) {
            return { selected: false, text: company.systemId, value: company.companyId };
        });
        var sysIdSet = new Set();
        var ddCompanyList = [];
        for (var index in ddCompaniesList) {
            if (sysIdSet.has(ddCompaniesList[index].text))
                continue;
            else{
                sysIdSet.add(ddCompaniesList[index].text);
                ddCompanyList.push(ddCompaniesList[index]);
            }
        }
        loginUI.model.CompanyDisplayList = ko.mapping.fromJS(ddCompanyList, { arrayUpdate: true, arrayInsert: true });
        var ddCompanyId = $('#SystemId').data("kendoDropDownList");
        ddCompanyId.dataSource.data(ddCompanyList);
        var selectCompanys = ddCompanyList.filter(function (i) { return sysCompanies.indexOf(i.value) > -1; })
        loginUI.ddCompanies = $('#CompanyId').data("kendoDropDownList").dataSource.data();
        ddCompanyId.select(0);
    },

    /**
     * data binding for company drop down list
     * @param {} companyList 
     * @returns {} 
     */
    bindCompaniesDropDown: function (companyList, selectedCompanyId) {
        loginUI.companiesSecureMap = {};
        loginUI.model.Companies = ko.mapping.fromJS(companyList, { arrayUpdate: true, arrayInsert: true });
        var ddCompanyList = companyList.map(function (company) {
            loginUI.companiesSecureMap[company.Id] = company.IsSecurityEnabled;
            return { selected: false, text: company.Name, value: company.Id };
        });
        loginUI.model.CompanyDisplayList = ko.mapping.fromJS(ddCompanyList, { arrayUpdate: true, arrayInsert: true });
        var ddCompanyId = $('#CompanyId').data("kendoDropDownList");
        ddCompanyId.dataSource.data(ddCompanyList);
        if (!selectedCompanyId) {
            ddCompanyId.select(0);
        } else {
            ddCompanyId.value(selectedCompanyId);
            setTimeout(loginUI.showPasswordField(selectedCompanyId));
        }
    }
};

var loginUICallback = {
    /**
     * This function is used to bind the companies dropdown list 
     * @param {} ret is call back data from server 
     * @returns {} 
     */
    getCompaniesSuccess: function (ret) {
        loginUI.pageInit = false;
        if (loginUI.model.ForAdmin()) {
            loginUIUnities.bindSysCompaniesDropDown(ret.companyList);
            return;
        }
        loginUIUnities.bindCompaniesDropDown(ret.companyList, ret.companyId);
    },

    /**
     * This function is used to handle the invailed user with error message
     * @param {} ret 
     * @returns {} 
     */
    getCompaniesFailedWithErrorMsg: function (ret) {
        loginUI.pageInit = true;
        if (loginUI.model.ForAdmin()) {
            loginUIUnities.bindSysCompaniesDropDown(ret.companyList);
        }
        loginUIUnities.bindCompaniesDropDown(ret.companyList, ret.companyId);
        $('#CompanyId').data("kendoDropDownList").value(ret.companyId);
        //pop-up the error
        if (ret.companyId && !loginUI.companiesSecureMap[ret.companyId] && loginUI.enterToLogin) {
            $("#btnLogin").click();
        } else {
            loginUI.enterToLogin = false;
            loginUI.displayMessage(ret.UserMessage.Errors);
        }
    },

    /**
     * This function is used to handle the invailed user without error message
     * @param {} ret 
     * @returns {} 
     */
    getCompaniesFailedWithoutErrorMsg: function (ret) {
        loginUI.pageInit = true;
        loginUIUnities.bindCompaniesDropDown(ret.companyList, ret.companyId);
        if (ret.companyId && !loginUI.companiesSecureMap[ret.companyId] && loginUI.enterToLogin) {
            $("#btnLogin").click();
        } else {
            loginUI.enterToLogin = false;
            setTimeout(function() {
                $("#txtPassword").focus()
            });
        }
    },

    /**
     * This function is used to handle user Id changed without focus blur
     * @param {} ret 
     * @returns {} 
     */
    updatedUserId: function (ret) {
        loginUIUnities.initCompaniesDropDown();
        loginUI.pageInit = true;
        loginUIUnities.bindCompaniesDropDown(ret.companyList, ret.companyId);
    },

    loginResult: function (jsonResult) {

        if (jsonResult != null) {
            if (jsonResult.IsSuccess) {
                // Success. Re-direct to home page now that credentials have been set
                var url = jsonResult.Url;
                if (loginUI.model.ForAdmin()) {
                    var systemId = $("#SystemId").data('kendoDropDownList').text();
                    url = url.replace("Core/Home", "AS/CustomScreen?id=Import&systemDbId=" + systemId);
                }
                window.location.replace(url);
            } else {
                // Not a success. Display errors/warnings or redirect
                if (jsonResult.PasswordExpires) {
                    loginUI.displayPasswordExpires(jsonResult);
                } else {
                    loginUI.displayMessage(jsonResult);
                }
            }
        }
    },

    changePasswordLink: function () {

        // Display Change Password Dialog
        loginUI.displayPassword();
    },

    changePasswordResult: function (jsonResult) {

        if (jsonResult != null) {
            if (jsonResult.IsSuccess) {
                // Success. Dismiss dialog
                loginUI.hidePassword();
            } else {
                // Not a success. Display errors/warnings and re-add click event for ok button
                loginUI.displayMessage(jsonResult);
                $("#btnPasswordOk").on('click', loginUI.changePassword);
            }
        }
    }

};


$(function () {
    loginUI.init(LoginViewModel);
});

affixFooter(); // initialize footer fix if no scrollbar
$(window).resize(affixFooter);

function affixFooter() {
    $('.footer').removeClass('affix-bottom').addClass(function () {
        if (window.innerHeight >= $('body').outerHeight(true)) return 'affix-bottom';
    });
}
