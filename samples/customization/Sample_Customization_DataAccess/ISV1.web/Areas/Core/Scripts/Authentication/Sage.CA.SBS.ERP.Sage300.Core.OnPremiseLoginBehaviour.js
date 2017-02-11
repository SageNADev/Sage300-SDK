/* Copyright (c) 1994-2015 Sage Software, Inc.  All rights reserved. */

"use strict";

var loginUI = loginUI || {};
loginUI = {
    model: {},

    // Main routine
    init: function (model) {

        // Allow up and down to also alter UI just as change event does
        // NOTE: up/down is regardless of change since change fires AFTER 
        // data has changed
        $("#CompanyId").kendoDropDownList().bind("change keyup", function (e) {
            // Show the password field for the seleted company?
            loginUI.showPasswordField($(this).val());
        });

        loginUI.initEvents();
        loginUI.initialLoad(model);

        // Set focus
        $("#txtUserId").focus();
    },

    // Init events
    initEvents: function () {

        // Sign in button
        $("#btnLogin").bind('click', function () {
            var data = {
                company: $("#CompanyId").val(),
                userId: $("#txtUserId").val(),
                password: $("#txtPassword").val()
            };
            loginRepository.login(data, loginUICallback.loginResult);
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
                } else if (currentControl === "" ||
                    currentControl === "txtUserId" ||
                    currentControl === "txtPassword") {
                    $("#btnLogin").click();
                    return false;
                } else if (currentControl === "txtPasswordUserId" ||
                    currentControl === "txtPasswordOld" ||
                    currentControl === "txtPasswordNew" || 
                    currentControl === "txtPasswordConfirm") {
                    loginUI.changePassword();
                    return false;
                } else {
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
            password: $("#txtPassword").val()
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
        $("#message").empty();
    },

    // Show message
    showMessage: function (html, jsonResult) {
        loginUI.clearSuccess();
        loginUI.clearMessage();

        $("#message").html(html);
        $("#message").show();

        // Add Event
        $("#btnMessage").on('click', jsonResult, loginUI.hideMessage);

        // Set focus
        $("#btnMessage").focus();
    },

    // Hide message
    hideMessage: function (jsonResult) {
        // Remove Event
        $("#btnMessage").off('click', loginUI.hideMessage);

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
            password: $("#txtPassword").val()
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
        loginUI.model = ko.mapping.fromJS(result);
        ko.applyBindings(loginUI.model);

        var company = loginUI.model.Company();

        // Select company in list
        $("#CompanyId").data('kendoDropDownList').value(company);

        // Show the password field for the seleted company?
        loginUI.showPasswordField(company);
    },

    // Determines password visibility
    showPasswordField: function (selectedCompany) {

        // Iterate companies
        $.each(loginUI.model.Companies(), function (index, company) {
            var id = company.Id();
            var isSecurityEnabled = company.IsSecurityEnabled();

            // Match selected company
            if (id === selectedCompany) {
                // Hide/Show based upon enabled security
                if (isSecurityEnabled) {
                    $("#passwordDiv").show();
                    $("#changePasswordDiv").show();
                } else {
                    $("#passwordDiv").hide();
                    $("#changePasswordDiv").hide();
                }
            } else {
                // No match. Therefore, skip
            }
        });

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

var loginUICallback = {

    loginResult: function (jsonResult) {

        if (jsonResult != null) {
            if (jsonResult.IsSuccess) {
                // Success. Re-direct to home page now that credentials have been set
                window.location.replace(jsonResult.Url);
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
