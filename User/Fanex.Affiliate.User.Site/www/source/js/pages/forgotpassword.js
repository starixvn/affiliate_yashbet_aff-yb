var ForgotPassword = (function ($) {
    var _me = {
        initialize: function () {
            _me.initEvent();
        },

        initEvent: function () {
            $("#forgotPasswordForm").on("submit", function (e) {
                _me.submit($(this));
                return false;
            })

            $("#btnReset").on("click", function () {
                let $email = $("#Email");
                let $container = $("#emailInputGroup");

                $email.val("");
                Site.toggleErrorMessage($container, true);
            })
        },

        submit: function ($form) {
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
                    success: function (response) {
                        _me.handleResponseResult(response);
                    },
                    beforeSend: function () {
                        Site.showLoader();
                    },
                    complete: function () {
                        Site.hideLoader();
                    }
                });
            }
        },

        handleResponseResult: function (response) {
            Site.displayResult(response.IsOk, response.Message);

            if (response.IsOk) {
                $("#mainContent").remove();
            }
        },

        checkValidEmail: function () {
            var $email = $("#Email");
            var isValid = Site.emailValidationPattern.test($email.val());

            var $container = $("#emailInputGroup");
            Site.toggleErrorMessage($container, isValid);

            return isValid;
        }
    }

    return _me;
})(jQuery)

ForgotPassword.initialize();