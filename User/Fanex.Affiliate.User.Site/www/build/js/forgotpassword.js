"use strict";

var ForgotPassword = (function ($) {
    var _me = {
        initialize: function initialize() {
            _me.initEvent();
        },

        initEvent: function initEvent() {
            $("#forgotPasswordForm").on("submit", function (e) {
                _me.submit($(this));
                return false;
            });

            $("#btnReset").on("click", function () {
                var $email = $("#Email");
                var $container = $("#emailInputGroup");

                $email.val("");
                Site.toggleErrorMessage($container, true);
            });
        },

        submit: function submit($form) {
            var isValidForm = _me.checkValidEmail();
            if (isValidForm) {
                var postData = $form.serializeObject();
                $.ajax({
                    type: "POST",
                    url: Site.resolveRelativeUrl("Password/Forgot"),
                    data: postData,
                    dataType: "json",
                    cache: false,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    success: function success(response) {
                        _me.handleResponseResult(response);
                    },
                    beforeSend: function beforeSend() {
                        Site.showLoader();
                    },
                    complete: function complete() {
                        Site.hideLoader();
                    }
                });
            }
        },

        handleResponseResult: function handleResponseResult(response) {
            Site.displayResult(response.IsOk, response.Message);

            if (response.IsOk) {
                $("#mainContent").remove();
            }
        },

        checkValidEmail: function checkValidEmail() {
            var $email = $("#Email");
            var isValid = Site.emailValidationPattern.test($email.val());

            var $container = $("#emailInputGroup");
            Site.toggleErrorMessage($container, isValid);

            return isValid;
        }
    };

    return _me;
})(jQuery);

ForgotPassword.initialize();

